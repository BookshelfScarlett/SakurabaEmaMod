using Microsoft.Xna.Framework.Media;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Theorafile;

namespace SakurabaEmaMod.Core.Readers.Ogv
{
    /// <summary>
    /// 禁用在调用ModSystem发起load前加载
    /// 要不然没法接入到tmod的asset读取
    /// </summary>
    [Autoload(false)]
    public sealed partial class OgvReader : IAssetReader, ILoadable
    {
        public const BindingFlags ReflectionFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        public static readonly string Extension = ".ogv";
        // 逻辑部分
        public static readonly Type videoType = typeof(Video);
        public async ValueTask<T> FromStream<T>(Stream stream, MainThreadCreationContext mainThreadCtx) where T : class
        {
            if (typeof(T) != videoType)
                throw AssetLoadException.FromInvalidReader<OgvReader, T>();
            // 将数据读入 MemoryStream ，因为TML的原始流在方法结束后会被 Dispose，
            // 而 Theora 需要在视频播放期间一直访问数据。
            MemoryStream memStream = new();
            await stream.CopyToAsync(memStream);
            memStream.Position = 0;
            // 使用GCHandle锁定这个Stream对象，不让GC回收它
            // 将GCHandle的IntPtr传给Theora作为datasource
            GCHandle handle = GCHandle.Alloc(memStream);
            try
            {
                // 切换到主线程创建 Video 对象 (FNA 资源通常需要主线程)
                await mainThreadCtx;
                return (CreateVideo(handle) as T)!;
            }
            catch
            {
                // 如果创建过程中出错，必须手动释放Handle和Stream，否则内存泄漏
                if (handle.IsAllocated)
                    handle.Free();
                memStream.Dispose();
                throw new Exception("加载视频时出错");
            }
        }
        public static Video CreateVideo(GCHandle streamHandle)
        {
            nint handlePtr = GCHandle.ToIntPtr(streamHandle);
            // 打开 Theora 流
            if (tf_open_callbacks(handlePtr, out nint theoraPtr, callbacks) != 0)
                throw new InvalidOperationException("无法通过 Theorafile 打开 OGV 流。");
            tf_videoinfo(theoraPtr, out int yWidth, out int yHeight, out double fps, out var fmt);
            // 使用空对象初始化，绕过构造函数
            Video result = (Video)RuntimeHelpers.GetUninitializedObject(videoType);
            // 注入数据
            int uvWidth = fmt == th_pixel_fmt.TH_PF_444 ? yWidth : yWidth / 2;
            int uvHeight = fmt == th_pixel_fmt.TH_PF_420 ? yHeight / 2 : yHeight;
            videoType.GetProperty("GraphicsDevice", ReflectionFlags)!.SetValue(result, Main.graphics.GraphicsDevice);
            videoType.GetField("theora", ReflectionFlags)!.SetValue(result, theoraPtr);
            videoType.GetField("yWidth", ReflectionFlags)!.SetValue(result, yWidth);
            videoType.GetField("yHeight", ReflectionFlags)!.SetValue(result, yHeight);
            videoType.GetField("uvWidth", ReflectionFlags)!.SetValue(result, uvWidth);
            videoType.GetField("uvHeight", ReflectionFlags)!.SetValue(result, uvHeight);
            videoType.GetField("fps", ReflectionFlags)!.SetValue(result, fps);
            videoType.GetProperty(nameof(Video.Duration), ReflectionFlags)!.SetValue(result, TimeSpan.MaxValue);
            videoType.GetField("needsDurationHack", ReflectionFlags)!.SetValue(result, true);
            return result;
        }
        void ILoadable.Load(Mod mod)
        {
            var assetReaderCollection = Main.instance.Services.Get<AssetReaderCollection>();
            assetReaderCollection.RegisterReader(this, Extension);
        }
        void ILoadable.Unload()
        {
            // 这个卸载Tml一般会自动处理，少用点反射了
        }
    }
}

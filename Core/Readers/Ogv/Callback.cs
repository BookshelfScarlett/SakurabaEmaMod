using ReLogic.Content.Readers;
using System;
using System.IO;
using System.Runtime.InteropServices;
using static Theorafile;

namespace SakurabaEmaMod.Core.Readers.Ogv
{
    public sealed partial class OgvReader : IAssetReader
    {
        #region 回调函数实现
        public static readonly tf_callbacks callbacks = new()
        {
            read_func = ReadCallback,
            seek_func = SeekCallback,
            close_func = CloseCallback,
        };
        // 这里的 datasource 就是传入的 GCHandle 指针
        public static unsafe nint ReadCallback(nint ptr, nint size, nint nmemb, nint dataSource)
        {
            try
            {
                GCHandle handle = GCHandle.FromIntPtr(dataSource);
                if (handle.Target is not MemoryStream stream)
                    return 0;
                int bytesToRead = (int)(nmemb * size);
                // 直接写入指针位置，无需中间 buffer
                Span<byte> span = new((void*)ptr, bytesToRead);
                return stream.Read(span);
            }
            catch
            {
                return 0;
            }
        }
        public static int SeekCallback(nint dataSource, long offset, SeekWhence whence)
        {
            try
            {
                GCHandle handle = GCHandle.FromIntPtr(dataSource);
                if (handle.Target is not MemoryStream stream)
                    return -1;
                SeekOrigin origin = whence switch
                {
                    SeekWhence.TF_SEEK_SET => SeekOrigin.Begin,
                    SeekWhence.TF_SEEK_CUR => SeekOrigin.Current,
                    SeekWhence.TF_SEEK_END => SeekOrigin.End,
                    _ => SeekOrigin.Begin
                };
                return (int)stream.Seek(offset, origin); // Theora 返回 0 代表成功
            }
            catch
            {
                return -1;
            }
        }
        public static int CloseCallback(nint dataSource)
        {
            try
            {
                GCHandle handle = GCHandle.FromIntPtr(dataSource);
                // 1. 获取 Stream 并 Dispose
                if (handle.Target is IDisposable stream)
                    stream.Dispose();
                // 2. 释放 Handle，允许 GC 回收 Stream 对象
                if (handle.IsAllocated)
                    handle.Free();
                return 0;
            }
            catch
            {
                return -1; // Error
            }
        }
        #endregion
    }
}

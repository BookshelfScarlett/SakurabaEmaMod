using SakurabaEmaMod.Menus.PVs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core.Hud
{
    /// <summary>
    /// 转场的图层处理放在了统一管理里面了
    /// </summary>
    public class CutsceneManager : ModSystem
    {
        internal static Queue<CutsceneHud> CutscenesQueue = new();
        /// <summary>
        /// 正在播放的专场
        /// </summary>
        internal static CutsceneHud ActiveCtuscenes;
        /// <summary>
        /// 如果当前有任何正在执行的专场。
        /// </summary>
        public static bool AnyActive => ActiveCtuscenes != null; 
        /// <summary>
        /// 为专场整了个队列排队的功能
        /// </summary>
        /// <param name="cutsceneHud"></param>
        public static void QueueCutscene(CutsceneHud cutsceneHud)
        {
            if (!Main.dedServ)
                CutscenesQueue.Enqueue(cutsceneHud);
        }
        /// <summary>
        /// 确认当前正在执行的转场与你传入的专场是否一致
        /// 这里的确认方式是通过模组内的一个Enum进行的
        /// 所以如果有新的转场，都要去手动更新那个enum
        /// </summary>
        /// <param name="cutsceneHud"></param>
        /// <returns></returns>
        public static bool IsActive(CutsceneHud cutsceneHud)
        {
            if (ActiveCtuscenes == null)
                return false;
            return ActiveCtuscenes.SetCutsceneType == cutsceneHud.SetCutsceneType;
        }
        public override void OnWorldUnload()
        {
            BloomVideo.IsUnloadWorld = true;
        }

        public override void PostUpdateEverything()
        {
            //专场理所当然不应该在服务器里运行
            if (Main.dedServ)
                return;
            if (ActiveCtuscenes == null)
            {
                if (CutscenesQueue.TryDequeue(out CutsceneHud result))
                {
                    ActiveCtuscenes = result;
                    ActiveCtuscenes.IsStart = true;
                    ActiveCtuscenes.OnStart();
                }
            }
            if (ActiveCtuscenes != null)
            {
                if (ActiveCtuscenes.IsStart)
                {
                    ActiveCtuscenes.OnStart();
                    return;
                }
                ActiveCtuscenes.Timer++;
                ActiveCtuscenes.Update();

                if (!ActiveCtuscenes.ShouldEndCutscene)
                    return;
                ActiveCtuscenes.IsEnd = true;
                ActiveCtuscenes.IsNowPlaying = false;
                //这里会先提前卡住一段时间，等OnEnd里面的内容确定执行完了才会正式释放资源
                //换句话说就是，你需要在onend这里手动去操作结束行为。
                ActiveCtuscenes.OnEnd();
                //准确来说是，假如你没有在OnEnd里手动给ShouldEndCutscene赋值为false
                //每次线程跑过来的时候，都会卡在OnEnd这个钩子里面运行
                //而在OnEnd里面真的执行了false时，则正常释放资源
                if (ActiveCtuscenes.ShouldEndCutscene)
                    return;
                ActiveCtuscenes = null;
            }
        }

    }
}

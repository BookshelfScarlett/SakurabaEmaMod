using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Cutscenes;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Globals.Players;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Instances
{
    public class ManosabaGlobalNPCs : GlobalNPC 
    {
        public override bool InstancePerEntity => true;
        //shorthand
        public Player LocalPlayer => Main.LocalPlayer;
        public ManosabaPlayer ModPlayer => LocalPlayer.ManosabaMod();
        public override void OnKill(NPC npc)
        {
            //不可能让下面的专场在玩家处播放。
            if (Main.dedServ)
                return;
            //这里走的是玩家的标记。
            if (ModPlayer.IsDoneFinalBossFight)
                return;
            switch (npc.type)
            {
                case NPCID.MoonLordCore:
                    if (!ManosabaClientConfig.Instance.NoFinalBloomPlay)
                        CutsceneManager.QueueCutscene(GetInstance<BloomCutScene>());
                    //我不建议扔到cutsecene的end里判断而是在这里判断
                    //究其原因是videoplayer的神奇代码导致这里视频的cutscene实际上比较危险
                    //在这里直接进行写入更好
                    ModPlayer.IsDoneFinalBossFight = true;
                    ManosabaMenuSystem.Instance.ReimplementJson(true);
                    break;
            }
        }
    }
}

using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.ManosabaScenes
{
    public class NoneScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Environment;
            }
        }
        public override int Music => new int?(MusicLoader.GetMusicSlot(Mod, ManosabaMusic.None)).Value;
        public override bool IsSceneEffectActive(Player player)
        {
            return CutsceneManager.AnyActive && player.ManosabaMod().IsPlayingBloom && !Main.dedServ;
        }
    }
}

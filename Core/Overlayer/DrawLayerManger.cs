using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Core.ParticleSystem;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core.Overlayer
{
    public class DrawLayerManger : ModSystem
    {
        public override void Load()
        {
            On_Main.DrawDust += BaseParticleManager.DrawParticles;
            On_Main.DrawInterface_36_Cursor += On_Main_DrawInterface_36_Cursor;

        }
        private void On_Main_DrawInterface_36_Cursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            CutsceneManager.ActiveCtuscenes?.Draw(Main.spriteBatch);
            orig();
        }


      
        public override void Unload()
        {
            On_Main.DrawDust -= BaseParticleManager.DrawParticles;
            On_Main.DrawInterface_36_Cursor -= On_Main_DrawInterface_36_Cursor;
        }
    }
}

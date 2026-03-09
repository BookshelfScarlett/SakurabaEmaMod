using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;

namespace SakurabaEmaMod.Menus.PVs
{
    public class BloomVideo
    {
        public static bool PlayBloom =false;
        public static bool IsDone;
        public static bool IsFullyDone;
        public static float InnerVolum = 1f;
        //是的孩子们这里有十几个控制动画的变量。史山就完事了你管他的聊那么多
        public Vector2 DrawPos = new Vector2(Main.screenWidth /2f, Main.screenHeight / 2f);
        public static bool IsUnloadWorld = false;
        public static void Update()
        {
            ManosabaMenu.BloomVideo.Volume = Main.musicVolume * ManosabaMenuUpdate.PVFlyIn;
            if(IsUnloadWorld)
            {
                ManosabaMenu.BloomVideo.Stop();
                IsUnloadWorld = false;
            }
            if (IsDone && ManosabaMenu.BloomVideo.State == MediaState.Stopped)
            {
                //先别急着出去倒是。等差不多了再说
                //你还是直接飞出去吧
                PlayBloom = false;
                InnerVolum = 1;
                IsDone = false;
            }   
            //先卡住一会，在完
            if (PlayBloom && ManosabaMenu.BloomVideo.State == MediaState.Stopped)
            {
                IsDone = true;
                ManosabaMenu.BloomVideo.Play(ManosabaVideo.BloomPV.Value);
            }
            //写这坨史山你看我干不干你就完事了
            if (PlayBloom || ManosabaMenuUpdate.ButtonsHoverOut > 0)
            {
                ManoHudManager.BlockAllUI = 2;
            }
            if (Main.mouseRight)
            {
                PlayBloom = false;
                ManosabaMenu.BloomVideo.Stop();
            }
        }
        public static bool IsPressingPageUp = false;
        public static bool IsPressingPageDown = false;
        public static void Draw()
        {
            if (!PlayBloom)
                return;
            Texture2D videoTexture = ManosabaMenu.BloomVideo.GetTexture();
            if (videoTexture != null)
            {
                Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                Vector2 initPos = new Vector2(0, Main.screenHeight / 2);
                Vector2 origin = videoTexture.Size() / 2;
                Main.spriteBatch.Draw(videoTexture, Vector2.Lerp(initPos, position, ManosabaMenuUpdate.PVFlyIn), null, Color.White * ManosabaMenuUpdate.PVFlyIn, 0f, origin, 0.85f, SpriteEffects.None, 0f);
            }
        }
    }
}

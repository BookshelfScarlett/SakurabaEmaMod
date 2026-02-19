using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Class;
using Terraria;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class ManosabaBackground
    {
        public static float DrawTimer = 0;
        public static float Progress = 0;
        public static ManosabaHud LoadGame => ManoHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManoHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManoHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManoHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud SwitchMenu => ManoHudManager.UICollection[GetInstance<SwitchMenu>().Type];

        static SpriteBatch SB { get => Main.spriteBatch; }
        public static  void Update()
        {
            //Progress++;
            //if(Progress <)
        }
        public static void DrawBackgound()
        {
            Texture2D backGround = ManosabaMenuAssets.Main_EmaBackground.Texture.Value;
            Texture2D logo = ManosabaMenuAssets.Main_Title.Texture.Value;
            SB.Draw(backGround, ManosabaMethods.ScreenCenter(), null, Color.White, 0, backGround.Size() / 2, 1f, 0, 0);
            Vector2 logoPos = new Vector2(Main.screenWidth - 375f, 200f);
            SB.Draw(logo, logoPos, null, Color.White, 0, logo.Size() / 2, 0.62f, 0, 0);
            Gallery.Draw(SB);
        }
    }
}

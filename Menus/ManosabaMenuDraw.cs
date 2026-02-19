using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SakurabaEmaMod.Menus
{
    /// <summary>
    /// 一个统一管理的类
    /// 本质上方便统一管理。
    /// </summary>
    public class ManosabaMenuDraw
    {
        #region 一大堆引用
        public static ManosabaHud LoadGame => ManoHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManoHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManoHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManoHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud SwitchMenu => ManoHudManager.UICollection[GetInstance<SwitchMenu>().Type];
        #endregion 
        public static SpriteBatch SB { get => Main.spriteBatch; }
        /// <summary>
        /// 绘制，这已经是这个主界面最简单的一步了……
        /// </summary>
        public static void PreDraw()
        {
            ManosabaMethods.EnterHudArea(BlendState.NonPremultiplied, SamplerState.LinearClamp);
            //背景
            DrawBackground();
            //主界面的按钮，不需要额外颜色。
            DrawButton();
            //退出
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
        public static void PostDraw()
        {
            //ManosabaMethods.EnterHudArea(BlendState.NonPremultiplied, SamplerState.LinearClamp);
            //DrawButton();
            //SB.End();
            //SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

        }
        private static void DrawButton()
        {
            //纯纯过滤确保一下
            if (Main.menuMode == ManosabaMenu.ID)
            {
                LoadGame.Draw(SB);
                Options.Draw(SB);
                Gallery.Draw(Main.spriteBatch);
                Exit.Draw(SB);
                SwitchMenu.Draw(SB);
            }
        }

        private static void DrawBackground()
        {
            Texture2D mask = ManosabaMenuAssets.Main_Mask.Texture.Value;
            //预先画一个艾玛背景，然后我们再在上面覆盖需要的mask
            Rectangle rectangle = new(0, 0, Main.screenWidth * 2, Main.screenHeight * 2);
            ManosabaBackground.DrawBackgound();
            Main.spriteBatch.Draw(mask, rectangle, Color.Black);
        }
    }
}

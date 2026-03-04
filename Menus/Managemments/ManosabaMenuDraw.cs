using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.AltMenu;
using SakurabaEmaMod.Menus.MainMenu;
using SakurabaEmaMod.Menus.PVs;
using Terraria;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.Managemments
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
        public static ManosabaHud LoadHud => ManoHudManager.UICollection[GetInstance<LoadHud>().Type];
        public static ManosabaHud GalleryHud => ManoHudManager.UICollection[GetInstance<GalleryHud>().Type];
        public static ManosabaHud RightArrow => ManoHudManager.UICollection[GetInstance<RightArrow>().Type];
        public static ManosabaHud LeftArrow => ManoHudManager.UICollection[GetInstance<LeftArrow>().Type];
        public static ManosabaHud Logo => ManoHudManager.UICollection[GetInstance<Logo>().Type];

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
            //主界面的按钮
            DrawButton();
            //其余的二级按钮
            DrawAltMenu();
            //退出
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            BloomVideo.Draw();
        }
        /// <summary>
        /// 用于在左上角绘制需要的文本内容
        /// 这里有个bug是如果局内切换语言，不会及时地出现改动
        /// 尽管如此，如果真的要改就过于HardWork了，而且也只是一个很小很小的问题，原版也有这个情况
        /// </summary>
        public static string DrawTextValue = "None";
        public static void PostDraw()
        {
            //这里的postdraw相当于是给原版的ui接入管理的
            //与自制的二级ui没有什么关系。
            if (Main.menuMode != ManosabaMenu.ID)
            {
                ManosabaMethods.EnterHudArea(BlendState.NonPremultiplied, SamplerState.LinearClamp);
                Texture2D cornorDeco = ManosabaMenuAssets.Alt_CornerDeco.Texture.Value;
                Texture2D altMenuBack = ManosabaMenuAssets.Alt_Mask.Texture.Value;
                SB.Draw(altMenuBack, ManosabaMethods.ScreenCenter(), null, Color.White * ManosabaMenuUpdate.GeneralFadingRatios, 0, altMenuBack.Size() / 2, .70f, 0, 0);
                SB.Draw(cornorDeco, new Vector2(200f, 100f), null, Color.White * ManosabaMenuUpdate.GeneralFadingRatios, 0, cornorDeco.Size() / 2, 0.55f, 0, 0);
                //误闯天家，我说泰拉的绘制就有问题xbzz
                DynamicSpriteFont dynamicSpriteFont = ManosabaFonts.等线.Value;
                Vector2 scale = new(1.0f);
                Vector2 Size = ChatManager.GetStringSize(dynamicSpriteFont, DrawTextValue, scale);
                Vector2 ori = Size / 2;
                //绘制的位置一定程度上需要偏移
                //考虑到这里只有一个横条按钮需要用到这个文本。直接硬编码
                Vector2 textPos = new(Size.X + 150f, 100f);
                for (int i = 0; i < 8; i++)
                    ChatManager.DrawColorCodedString(SB, dynamicSpriteFont, DrawTextValue, textPos + ToRadians(60f * i).ToRotationVector2() * 2f, Color.Black , 0, ori, scale);
                ChatManager.DrawColorCodedString(SB, dynamicSpriteFont, DrawTextValue, textPos, Color.White, 0, ori, scale);
                
                SB.End();
                SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            }
        }
        private static void DrawAltMenu()
        {
            if (Main.menuMode == ManosabaMenu.ID)
            {
                GalleryHud.Draw(SB);
                LoadHud.Draw(SB);
            }
        }
        private static void DrawButton()
        {
            //纯纯过滤确保一下
            //这下面的按钮是无论出于啥状态下都会一直在绘制的。
            //因为这里用的是一个透明的背景
            LoadGame.Draw(SB);
            Options.Draw(SB);
            Gallery.Draw(SB);
            Exit.Draw(SB);
            SwitchMenu.Draw(SB);
            //SwitchManosabaBackground.Draw(SB);
            LeftArrow.Draw(SB);
            RightArrow.Draw(SB);
            Logo.Draw(SB);
        }

        private static void DrawBackground()
        {
            //预先画一个艾玛背景，然后我们再在上面覆盖需要的mask
            ManosabaBackground.DrawBackgound();
        }
    }
}

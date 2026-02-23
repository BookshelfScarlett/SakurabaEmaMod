using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.AltMenu
{
    public class GalleryHud : ManosabaHud
    {
        public static bool IsEnable = false;
        public static bool IsFading = true;
        public static ManosabaHud Achievement => ManoHudManager.UICollection[GetInstance<Achievement>().Type];
        public static ManosabaHud Credits => ManoHudManager.UICollection[GetInstance<Credit>().Type];
        public static ManosabaHud CancelGallery => ManoHudManager.UICollection[GetInstance<CancelGallery>().Type];
        public override int UIDepth => 2;
        public override bool PreSetDepth() => IsEnable;
        public override void PostUpdate()
        {
            //只有完全渐变完成的时候，这里才会允许设定为true的可能
            //主要是为了短暂的卡一下按下esc的玩家避免直接完全退出了主界面
            if (Main.keyState.IsKeyDown(Keys.Escape) && !ManosabaMenuUpdate.BanSwitchMenu)

                IsFading = true;
            Rectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            if (IsEnable && !IsFading)
            {
                Opacity = Lerp(Opacity, 1f, 0.2f);

            }
            if (IsFading && Main.keyState.IsKeyUp(Keys.Escape))
            {
                Opacity = Lerp(Opacity, 0f, 0.2f);
            }
            if (IsEnable)
            {
                Achievement.Update();
                Credits.Update();
                //非常奇异地遇到了崩溃问题。
                //现在知道为什么了。
                CancelGallery.Update();
                UpdateButtons();
            }
            else
            {
                Opacity = Lerp(Opacity, 0f, 0.1f);
                IsFading = false;
            }
            if (Opacity < 0.02f)
                IsEnable = false;
            Achievement.Opacity = Opacity;
            Credits.Opacity = Opacity;
            CancelGallery.Opacity = Opacity;
        }
        public override void OnRightClick()
        {
            //IsFading = true;
        }
        private void UpdateButtons()
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsEnable)
            {
                string TextValue = Mod.GetLocalizationKey("Menu.GalleryText").ToLangValue();
                //背景仍然需要单独的绘制
                //待会处理一下。
                Texture2D cornorDeco = ManosabaMenuAssets.Alt_CornerDeco.Texture.Value;
                Texture2D altMenuBack = ManosabaMenuAssets.Alt_Mask.Texture.Value;

                spriteBatch.Draw(altMenuBack, ManosabaMethods.ScreenCenter(), null, Color.White * Opacity, 0, altMenuBack.Size() / 2, 0.70f, 0, 0);
                spriteBatch.Draw(cornorDeco, new Vector2(200f, 100f), null, Color.White * Opacity, 0, cornorDeco.Size() / 2, 0.55f, 0, 0);

                //误闯天家，我说泰拉的绘制就有问题xbzz
                DynamicSpriteFont dynamicSpriteFont = ManosabaFonts.等线.Value;
                Vector2 scale = new(1.0f);
                Vector2 Size = ChatManager.GetStringSize(dynamicSpriteFont, TextValue, scale);
                Vector2 ori = Size / 2;
                //绘制的位置一定程度上需要偏移
                //考虑到这里只有一个横条按钮需要用到这个文本。直接硬编码
                Vector2 textPos = new(270f, 100f);
                ChatManager.DrawColorCodedString(spriteBatch, dynamicSpriteFont, TextValue, textPos, Color.White * Opacity, 0, ori, scale);

                Credits.Draw(spriteBatch);
                Achievement.Draw(spriteBatch);
                CancelGallery.Draw(spriteBatch);
            }
        }
    }
}

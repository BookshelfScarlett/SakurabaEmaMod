using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.AltMenu
{
    public class LoadHud : ManosabaHud
    {
        public static bool IsEnable = false;
        public static bool IsFading = true;
        public static ManosabaHud SinglePlayer => ManosabaHudManager.UICollection[GetInstance<SinglePlayer>().Type];
        public static ManosabaHud Multiplayer => ManosabaHudManager.UICollection[GetInstance<Multiplayer>().Type];
        public static ManosabaHud Workshop => ManosabaHudManager.UICollection[GetInstance<Workshop>().Type];
        public static ManosabaHud CancelLoad => ManosabaHudManager.UICollection[GetInstance<CancelLoad>().Type];
        public override int UIDepth => 2;
        public override bool PreSetDepth() => IsEnable;
        public override void PostUpdate()
        {
            //只有完全渐变完成的时候，这里才会允许设定为true的可能
            //主要是为了短暂的卡一下按下esc的玩家避免直接完全退出了主界面
            if ((Main.keyState.IsKeyDown(Keys.Escape) || Main.mouseRight) && !ManosabaMenuUpdate.BanSwitchMenu)
            {
                IsFading = true;
            }
            Rectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            if (IsEnable && !IsFading)
            {
                Opacity = Lerp(Opacity, 1f, 0.2f);
                
            }
            //只有松开esc，才会允许Opacity的递减
            //这里仍然有个问题是如果玩家持续不断按住esc，仍然会退出到主界面
            //尽管如此，这是个待办事项。
            if (IsFading && (Main.keyState.IsKeyUp(Keys.Escape) || Main.mouseRightRelease))
            {
                Opacity = Lerp(Opacity, 0f, 0.2f);
            }
            if (IsEnable)
            {
                SinglePlayer.Update();
                Multiplayer.Update();
                Workshop.Update();
                CancelLoad.Update();
                UpdateButtons();
            }
            else
            {
                Opacity = Lerp(Opacity, 0f, 0.1f);
                IsFading = false;
            }
            if (Opacity < 0.02f)
                IsEnable = false;
            SinglePlayer.Opacity = Opacity;
            Multiplayer.Opacity = Opacity;
            Workshop.Opacity = Opacity;
            CancelLoad.Opacity = Opacity;
        }
        public override void OnRightClick()
        {
        }
        private void UpdateButtons()
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsEnable)
            {
                string TextValue = Mod.GetLocalizationKey("Menu.LoadText").ToLangValue();
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
                Vector2 ori = new(Size.X,Size.Y);
                //绘制的位置一定程度上需要偏移
                //考虑到这里只有一个横条按钮需要用到这个文本。直接硬编码
                Vector2 textPos = new(310f, 120f);
                for (int i = 0; i < 8; i++)
                    ChatManager.DrawColorCodedString(spriteBatch, dynamicSpriteFont, TextValue, textPos + ToRadians(60f * i).ToRotationVector2() * 2f, Color.Black * Opacity, 0, ori, scale);
                ChatManager.DrawColorCodedString(spriteBatch, dynamicSpriteFont, TextValue, textPos, Color.White * Opacity, 0, ori, scale);

                SinglePlayer.Draw(spriteBatch);
                Multiplayer.Draw(spriteBatch);
                Workshop.Draw(spriteBatch);
                CancelLoad.Draw(spriteBatch);
            }
        }
    }
}

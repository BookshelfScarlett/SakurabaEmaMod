using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.Class;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class LoadGame : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_LoadGameChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_LoadGameUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(0, 200f);
        public override int TargetMenuID => MenuID.None;
        public override void OnMouseLeftRelease()
        {
            //需要进入alt的二级ui
            //这里还没设置，待会做二级ui的时候开始弄
            base.OnMouseLeftRelease();
        }
    }
    /// <summary>
    /// 画廊，目前用于跳转成就界面
    /// </summary>
    public class Gallery : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_GalleryChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_GalleryUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(Main.screenWidth /2, Main.screenHeight / 2);
        //成就的iD名呢？
        public override int TargetMenuID => MenuID.None;
        public override void OnMouseLeftRelease()
        {
            base.OnMouseLeftRelease();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D useTex = MouseIsHovering ? ChosenTexture: UnChosenTexture;
            Vector2 posOffset = MouseIsHovering ? TexturePosOffset : Vector2.Zero;
            //实际绘制
            spriteBatch.Draw(useTex, Center, Rectangle, DrawColor, Rotation, useTex.Size() / 2, 1f, 0, 0);
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = ChatManager.GetStringSize(font, "1145", Vector2.One);
            ChatManager.DrawColorCodedString(spriteBatch, font, "1145", Center, Color.White, 0, size / 2, Vector2.One * Scale2 * 1.5f);

        }
    }
    public class Options : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_OptionChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_OptionUnChosen.Texture.Value;
        public override Vector2 Center => base.Center;
        public override int TargetMenuID => MenuID.Settings;

    }
    public class Exit : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_ExitChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_ExitUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(200f, Main.screenHeight - 500f);
        public override void OnMouseLeftRelease()
        {
            Main.instance.Exit();
        }
    }
    public class SwitchMenu : ManosabaHud
    {
        //根据代码情况来看这里的码实际上是tmod提供的
        //反正复制无罪
        public string TextValue;
        public override void PostUpdate()
        {
            ModMenu currentMenu = MenuLoader.CurrentMenu;
            int newMenus;
            lock (MenuLoader.menus)
            {
                string[] knownMenus = MenuLoader.KnownMenus;
                foreach (ModMenu menu in MenuLoader.menus)
                {
                    menu.IsNew = menu.IsAvailable && !knownMenus.Contains(menu.FullName);
                }
                newMenus = MenuLoader.menus.Count((ModMenu m) => m.IsNew);
            }
            Position = new Vector2(Main.screenWidth / 2, Main.screenHeight - 20);
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextValue = $"{Language.GetTextValue("tModLoader.ModMenuSwap")}: {currentMenu.DisplayName}{(newMenus == 0 ? "" : ModLoader.notifyNewMainMenuThemes ? $" ({newMenus} New)" : "")}";
            Vector2 size = ChatManager.GetStringSize(font, ChatManager.ParseMessage(TextValue, DrawColor).ToArray(), Vector2.One);
            Rectangle = Utils.CenteredRectangle(Position, size);
        }
        public override void StartHover()
        {
        }
        public override void MouseHover(bool isHover)
        {
            if (isHover)
            {
                if (Main.mouseLeft || Main.mouseRight)
                    Scale2 = Lerp(Scale2, 0.95f, 0.2f);
                else
                    Scale2 = Lerp(Scale2, 1.05f, 0.2f);
            }
            else
            {
                Scale2 = Lerp(Scale2, 1f, 0.2f);
            }
        }
        public override void OnMouseLeftRelease()
        {
            MenuLoader.OffsetModMenu(1);
        }
        public override void OnMouseRightRelease()
        {
            MenuLoader.OffsetModMenu(-1);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = ChatManager.GetStringSize(font, TextValue, Vector2.One);
            ChatManager.DrawColorCodedString(spriteBatch, font, TextValue, Position, Color.Silver, 0, size / 2, Vector2.One * Scale2);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.AltMenu;
using SakurabaEmaMod.Menus.Class;
using SakurabaEmaMod.Menus.Managemments;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class LoadGame : ManosabaButtonClass
    {
        public static float GeneralWidthOffset = 0f;
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_LoadGameChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_LoadGameUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(250f, Main.screenHeight - 150f);
        public override int TargetMenuID => MenuID.None;
        public override float ButtonScale => 0.85f;
        public override Rectangle Hitbox => Utils.CenteredRectangle(Position, new Vector2(300, 200));
        public override void OnMouseLeftRelease()
        {
            //播放BloomPV的时候做掉这里避免眼疾手快时意外退出
            if (PVs.BloomVideo.PlayBloom)
                return;

            //需要进入alt的二级ui
            //这里还没设置，待会做二级ui的时候开始弄
            if (!ManoHudManager.ActiveDepth[2])
            {
                SoundEngine.PlaySound(ManosabaSounds.Menu_MainChoice);
                ManosabaMenuUpdate.GeneralFadingRatios = 0;
                LoadHud.IsEnable = true;
            }
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
        public override Vector2 Center => new Vector2(Main.screenWidth / 2 - 300f, Main.screenHeight - 75f);
        //成就的iD名呢？
        public override int TargetMenuID => MenuID.None;
        public override float ButtonScale => 0.65f;
        public override Rectangle Hitbox => base.Hitbox;
        public override void OnMouseLeftRelease()
        {
            //播放BloomPV的时候做掉这里避免眼疾手快时意外退出
            if (PVs.BloomVideo.PlayBloom)
                return;

            //这里还没设置，待会做二级ui的时候开始弄
            if (!ManoHudManager.ActiveDepth[2])
            {
                //需要给渐变。
                SoundEngine.PlaySound(ManosabaSounds.Menu_MainChoice);
                ManosabaMenuUpdate.GeneralFadingRatios = 0;
                GalleryHud.IsEnable = true;
            }
        }
    }
    public class Options : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_OptionChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_OptionUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(Main.screenWidth / 2f - 150f, Main.screenHeight - 125f);
        public override int TargetMenuID => MenuID.Settings;
        public override float ButtonScale => 0.65f;
        public override Rectangle Hitbox => base.Hitbox;
        public override void OnMouseLeftRelease()
        {
            //播放BloomPV的时候做掉这里避免眼疾手快时意外退出
            if (PVs.BloomVideo.PlayBloom)
                return;

            SoundEngine.PlaySound(ManosabaSounds.Menu_MainChoice);
            ManosabaMenuDraw.DrawTextValue = Mod.GetLocalizationKey("Menu.Options").ToLangValue();
            ManosabaMenuUpdate.GeneralFadingRatios = 0;
            ManosabaMenuMethods.ChangeMenu(TargetMenuID);
        }

    }
    public class Exit : ManosabaButtonClass
    {
        public override bool ShouldDrawText => false;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_ExitChosen.Texture.Value;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_ExitUnChosen.Texture.Value;
        public override Vector2 Center => new Vector2(Main.screenWidth / 2f - 30f, Main.screenHeight - 75f);
        public override float ButtonScale => 0.65f;
        public override Rectangle Hitbox => base.Hitbox;
        public override void OnMouseLeftRelease()
        {
            //播放BloomPV的时候做掉这里避免眼疾手快时意外退出
            if (PVs.BloomVideo.PlayBloom)
                return;
            Main.instance.Exit();
        }
    }
    public class SwitchMenu : ManosabaHud
    {
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
            if (ManosabaMenu.CanSwitchToOtherMenu)
                MenuLoader.OffsetModMenu(1);
        }
        public override void OnMouseRightRelease()
        {
            if (ManosabaMenu.CanSwitchToOtherMenu)
                MenuLoader.OffsetModMenu(-1);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //这个Ratios是从0到1的，因此这里也是逆向的
            float ratios = (1 - ManosabaMenuUpdate.ButtonsHoverOut);
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = ChatManager.GetStringSize(font, TextValue, Vector2.One);
            ChatManager.DrawColorCodedString(spriteBatch, font, TextValue, Position, Color.Silver * ratios, 0, size / 2, Vector2.One * Scale2);

            //在这里画模组的版本字号。

            Vector2 scale = new(1.15f);
            string TextValue2 = Mod.GetLocalizationKey("Menu.ModVersion").ToLangValue();
            font = ManosabaFonts.BookAntiqua.Value;
            Vector2 size2 = ChatManager.GetStringSize(font, TextValue, Vector2.One * scale);
            Vector2 ori = size2 / 2;
            Vector2 pos = new Vector2(Main.screenWidth, Main.screenHeight - 50f) + Vector2.UnitX * 140f;
            ChatManager.DrawColorCodedString(Main.spriteBatch, font, TextValue2, pos, Color.White * ratios, 0, ori, Vector2.One * scale);

        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
{
    public class ManosabaMenu : ModMenu
    {
        /// <summary>
        /// 需要给所有的主界面分配一个独立ID
        /// 这里的ID取自魔裁的发布日期。20250718
        /// 我不太相信会有神人跟这个主界面的ID一样……
        /// </summary>
        public static int ID = 20250718;
        /// <summary>
        /// 出于某些神秘的原因这里所有的原版主界面贴图都是要直接被干掉的
        /// 反正就是这么一回事。
        /// </summary>
        public override Asset<Texture2D> SunTexture => ManosabaTexture.InvisAsset.Texture;
        public override Asset<Texture2D> MoonTexture => ManosabaTexture.InvisAsset.Texture;
        public override Asset<Texture2D> Logo => ManosabaTexture.InvisAsset.Texture;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => null;
        public override int Music => MusicLoader.GetMusicSlot(ManosabaMusic.Menu);
        public override string DisplayName => Mod.GetLocalizationKey("Menu.Name").ToLangValue();
        public static bool CanSwitchToOtherMenu;
        public override void OnSelected()
        {
            //选择当前主界面时的行为
            //然后依据文档的名字来修改对应的ID
            Main.menuMode = ID;
            //这下面用于处理背景的渐变。
            //模拟魔裁进入游戏的情况
            ManosabaBackground.TheScaleRatios = 0;
            ManosabaBackground.LogoScaleRatios = 0;
            ManosabaMenuUpdate.GeneralFadingRatios = 0;
            ManosabaMenuLayer.OverlayBlackOpacity = 1;
            CanSwitchToOtherMenu = false;
        }
        public override void OnDeselected()
        {
            CanSwitchToOtherMenu = false;
            if (Main.menuMode != MenuID.FancyUI)
                Main.menuMode = MenuID.Title;
        }
        /// <summary>
        /// 同样预制钩子。
        /// </summary>
        /// <param name="isOnTitleScreen"></param>
        public override void Update(bool isOnTitleScreen)
        {
            if (Main.mouseLeftRelease && Main.mouseRightRelease)
            {
                CanSwitchToOtherMenu = true;
            }
            ManosabaMenuUpdate.CustomUpdate();
        }
        /// <summary>
        /// 先画出来再看看我测
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="logoDrawCenter"></param>
        /// <param name="logoRotation"></param>
        /// <param name="logoScale"></param>
        /// <param name="drawColor"></param>
        /// <returns></returns>
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            ManosabaMenuDraw.PreDraw();
            return false;
        }
        public override void PostDrawLogo(SpriteBatch spriteBatch, Vector2 logoDrawCenter, float logoRotation, float logoScale, Color drawColor)
        {
            ManosabaMenuDraw.PostDraw();
        }

    }
}

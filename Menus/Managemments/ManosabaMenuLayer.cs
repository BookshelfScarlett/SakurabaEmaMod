using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
{
    public class ManosabaMenuLayer : ModSystem
    {
        public static float OverlayBlackOpacity = 0f;
        public override void Load()
        {
            On_Main.DrawMenu += PostDrawMenu;
            On_Main.DrawVersionNumber += On_DrawVersion;
            On_Main.DrawSocialMediaButtons += On_DrawSocialMediaButtons;
            On_Main.DrawtModLoaderSocialMediaButtons += On_DrawtModLoaderSocialMediaButtons;
            On_Main.HandleNews += On_HandleNews;
        }
        public override void Unload()
        {
            On_Main.DrawMenu -= PostDrawMenu;
            On_Main.DrawVersionNumber -= On_DrawVersion;
            On_Main.DrawSocialMediaButtons -= On_DrawSocialMediaButtons;
            On_Main.DrawtModLoaderSocialMediaButtons -= On_DrawtModLoaderSocialMediaButtons;
            On_Main.HandleNews -= On_HandleNews;
        }
        public static void PostDrawMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gametime)
        {
            orig(self, gametime);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            OverlayBlackOpacity = Clamp(OverlayBlackOpacity, 0f, 1f);
            if (Main.menuMode != MenuID.Title && OverlayBlackOpacity > 0.02f)
            {
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * OverlayBlackOpacity);
            }
            Main.spriteBatch.End();
        }
        public static void On_DrawVersion(On_Main.orig_DrawVersionNumber orig, Color menuColor, float upBump)
        {
            if (MenuLoader.CurrentMenu is ManosabaMenu)
            {
            }
            else
            {
                orig(menuColor, upBump);
            }
        }
        public static void On_DrawSocialMediaButtons(On_Main.orig_DrawSocialMediaButtons orig, Color menuColor, float upBump)
        {
            if (MenuLoader.CurrentMenu is ManosabaMenu)
            {
            }
            else
            {
                orig(menuColor, upBump);
            }
        }
        public static void On_DrawtModLoaderSocialMediaButtons(On_Main.orig_DrawtModLoaderSocialMediaButtons orig, Color menuColor, float upBump)
        {
            if (MenuLoader.CurrentMenu is ManosabaMenu)
            {
            }
            else
            {
                orig(menuColor, upBump);
            }
        }
        public static void On_HandleNews(On_Main.orig_HandleNews orig, Color menuColor)
        {
            if (MenuLoader.CurrentMenu is ManosabaMenu)
            {
            }
            else
            {
                orig(menuColor);
            }
        }
    }
}

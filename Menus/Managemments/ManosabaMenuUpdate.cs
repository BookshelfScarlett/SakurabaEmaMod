using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.AltMenu;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.AccessControl;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
{
    /// <summary>
    /// 统一的更新管理。
    /// </summary>
    public class ManosabaMenuUpdate
    {
        public static ManosabaHud LoadGame => ManoHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManoHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManoHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManoHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud LoadHud => ManoHudManager.UICollection[GetInstance<LoadHud>().Type];
        public static ManosabaHud GalleryHud => ManoHudManager.UICollection[GetInstance<GalleryHud>().Type];
        public static ManosabaHud SwitchMenu => ManoHudManager.UICollection[GetInstance<SwitchMenu>().Type];
        public static ManosabaHud RightArrow => ManoHudManager.UICollection[GetInstance<RightArrow>().Type];
        public static ManosabaHud LeftArrow => ManoHudManager.UICollection[GetInstance<LeftArrow>().Type];
        //这里也同样是会做一些处理的。
        public static bool ToManosabaMenu = false;
        public static bool ToOtherMenu = false;
        public static bool BanSwitchMenu = false;
        /// <summary>
        /// 上一界面的ID
        /// </summary>
        public static int LastMenuID = -1;
        public static int NextMenuID = -1;
        public static List<Action> OnChangeToTargetMenuID = [];
        public static float GeneralFadingRatios = 0f;
        public static void CustomUpdate()
        {
            UpdateFading();
            if (Main.menuMode == ManosabaMenu.ID)
            {
                LoadGame.Update();
                Options.Update();
                Gallery.Update();
                Exit.Update();
                SwitchMenu.Update();
                LoadHud.Update();
                GalleryHud.Update();
                //背景完全显示出来之间禁用背景切换
                if (ManosabaBackground.LogoScaleRatios < 1f)
                    return;
                LeftArrow.Update();
                RightArrow.Update();
            }
        }
        private static void DrawFading()
        {
            //事实上，这里只会管绘制，不会影响按钮的正常使用。
            ManosabaBackground.Update();
        }
        private static void UpdateFading()
        {
            //Clamp掉这个ratios
            GeneralFadingRatios = Clamp(GeneralFadingRatios, 0, 1);
            float frames = 8f;
            ref float BlackLayer = ref ManosabaMenuLayer.OverlayBlackOpacity;
            //BanSwitchMenu会随时更新，确保不会出现意外
            BanSwitchMenu = BlackLayer > 0.01f;
            //只会覆盖原版主界面的时候展现背景
            //因为原版会优先切换到主界面，随后拦截主界面并标记已经切换到了自定义UI
            if (Main.menuMode == MenuID.Title)
                ToManosabaMenu = true;
            else
                LastMenuID = Main.menuMode;
            //从原版主界面至需要的魔裁界面，先不切换菜单，等淡出完成后再切换到主界面
            if (ToManosabaMenu)
            {
                if (Main.menuMode != LastMenuID)
                    Main.menuMode = LastMenuID;
                GeneralFadingRatios += 1 / frames;
                BlackLayer += 1 / frames;
                if (BlackLayer > 0.98f)
                {
                    ToManosabaMenu = false;
                    Main.menuMode = ManosabaMenu.ID;
                }
            }
            else if (ToOtherMenu)
            {
                //切换至其他界面
                //原游戏由主界面到其他界面没有渐变，我们这也不会做渐变。
                //我草他妈有
                GeneralFadingRatios += 1 / frames;
                BlackLayer += 1 / frames;
                DrawFading();
                if (GeneralFadingRatios >= 1f)
                {
                    Main.menuMode = NextMenuID;
                    //设定为1，因为仍然需要绘制背景
                    GeneralFadingRatios = 1;
                    NextMenuID = -1;
                    ToOtherMenu = false;
                    if (OnChangeToTargetMenuID.Count != 0)
                    {
                        for (int i = 0; i < OnChangeToTargetMenuID.Count; i++)
                        {
                            Action action = OnChangeToTargetMenuID[i];
                            action();
                        }
                        OnChangeToTargetMenuID.Clear();
                    }
                }
            }
            else //默认会慢慢淡出
            {
                DrawFading();
                //第一次加载魔裁主界面时，我们才用非常慢的缓动
                BlackLayer -= 1 / 15f;
            }
        }
    }
}

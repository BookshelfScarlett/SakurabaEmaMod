using Microsoft.Xna.Framework;
using Newtonsoft.Json.Converters;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.AltMenu;
using SakurabaEmaMod.Menus.MainMenu;
using SakurabaEmaMod.Menus.PVs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

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
        public static ManosabaHud Logo => ManoHudManager.UICollection[GetInstance<Logo>().Type];
        //这里也同样是会做一些处理的。
        public static bool ToManosabaMenu = false;
        public static bool ToOtherMenu = false;
        public static bool BanSwitchMenu = false;
        /// <summary>
        /// 上一界面的ID
        /// </summary>
        public static int LastMenuID = -1;
        /// <summary>
        /// 下一个界面的ID
        /// </summary>
        public static int NextMenuID = -1;
        /// <summary>
        /// 我也不知道
        /// </summary>
        public static List<Action> OnChangeToTargetMenuID = [];
        /// <summary>
        /// 总的渐变，不过这个要重构了
        /// </summary>
        public static float GeneralFadingRatios = 0f;
        /// <summary>
        /// /按钮的渐入渐出，用于pv播放和长时间挂机，或者别的
        /// </summary>
        public static float ButtonsHoverOut = 0;
        /// <summary>
        /// pv渐入，专门给pv用的。
        /// </summary>
        public static float PVFlyIn = 0;
        /// <summary>
        /// 挂机状态
        /// </summary>
        public static float IdleTimer = 0;
        /// <summary>
        /// 是否处于挂机状态
        /// </summary>
        public static bool IsIdling;
        /// <summary>
        /// 指针上一个所在位置
        /// </summary>
        public static Vector2 LastAnchorPosition = Vector2.Zero;
        public static void CustomUpdate()
        {
            if (Main.menuMode == ManosabaMenu.ID)
                UpdateButtons();
            //当前有任何正在执行的二级UI都不要尝试做这个行为。
            if (!ManoHudManager.ActiveDepth[2])
            {
                IdleTimer++;

                //是的孩子们我一直在偷偷吃你的性能
                //哈哈。
                //在主界面挂机超过30秒的时候直接挂机
                //对。
                if (IdleTimer > GetSeconds(50) && !IsIdling)
                {
                    IsIdling = true;
                    LastAnchorPosition = Main.MouseScreen;
                    //记录玩家当前鼠标位置
                }

                if (IsIdling)
                {
                    //正式进入挂机状态的时候执行的逻辑
                    //这里会让主界面大部分元素都转移出去只留一个背景本身
                    ButtonsHoverOut = Lerp(ButtonsHoverOut, 1f, 0.1f);
                    if (ButtonsHoverOut > 0.98f)
                        ButtonsHoverOut = 1;
                    if ((Main.MouseScreen - LastAnchorPosition).LengthSquared() > 100f * 100f)
                    {
                        IsIdling = false;
                        IdleTimer = 0;
                    }

                    return;
                }
            }
            //播放视频的时候会直接阻止这里的传值
            //这史山你看我干不干你就完事了
            if (!BloomVideo.PlayBloom)
                UpdateNotBloomcases();
            else
                UpdateBloomCases();
            
            BloomVideo.Update();
        }
        private static void IdleUpdate()
        {

        }
        private static void UpdateButtons()
        {
                LoadGame.Update();
                Options.Update();
                Gallery.Update();
                Exit.Update();
                SwitchMenu.Update();
                LoadHud.Update();
                GalleryHud.Update();
                //背景完全显示出来之间禁用背景切换
                Logo.Update();
                if (Logo.Opacity < 1)
                    return;
                LeftArrow.Update();
                RightArrow.Update();

        }
        private static void UpdateNotBloomcases()
        {
            UpdateFading();
            if (ButtonsHoverOut > 0.02f)
                ButtonsHoverOut = Lerp(ButtonsHoverOut, 0f, 0.1f);
            else
                ButtonsHoverOut = 0;

            if (PVFlyIn > 0.02f)
                PVFlyIn = Lerp(PVFlyIn, 0f, 0.1f);
            else
                PVFlyIn = 0;

        }
        //我写的什么玩意
        private static void UpdateBloomCases()
        {

            ButtonsHoverOut = Lerp(ButtonsHoverOut, 1f, 0.1f);
            ButtonsHoverOut = Clamp(ButtonsHoverOut, 0, 1);
            if (ButtonsHoverOut > 0.98f)
            {
                ButtonsHoverOut = 1f;
                PVFlyIn = Lerp(PVFlyIn, 1f, 0.1f);
                if (PVFlyIn > 0.98f)
                    PVFlyIn = 1f;
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Managemments;
using Steamworks;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.Engine;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class ManosabaBackground
    {
        public static float DrawTimer = 0;
        public static float Progress = 0;
        public static ManosabaHud LoadGame => ManosabaHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManosabaHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManosabaHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManosabaHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud SwitchMenu => ManosabaHudManager.UICollection[GetInstance<SwitchMenu>().Type];
        static SpriteBatch SB { get => Main.spriteBatch; }
        /// <summary>
        /// 默认艾玛背景
        /// </summary>
        public static Texture2D UseTex = ManosabaMenuAssets.Still_Ema.Texture.Value;
        public static bool CanChangeMenu = false;
        public static float TheScaleRatios = 0f;
        public static float BackgroundFading = 0f;
        private const ulong AuthorSteamID = 76561198813687655;
        private static List<string> AvailableName = ["kino"];
        public static void Update()
        {
            UseTex = GetBackgroundOnNeed();
            TheScaleRatios = Lerp(TheScaleRatios, 1f, 0.04f);
            if (TheScaleRatios > 0.98f)
                TheScaleRatios = 1f;
        }
        public static int CurrentBackgroundID = ManosabaMenuID.Ema;
        private static readonly Dictionary<int, int> _menuIDMap = new()
        {
            { ManosabaMenuID.Ema, ManosabaMenuID.Ema },
            { ManosabaMenuID.Hiro, ManosabaMenuID.Hiro },
            { ManosabaMenuID.Anan, ManosabaMenuID.Anan },
            { ManosabaMenuID.Noah, ManosabaMenuID.Noah},
            { ManosabaMenuID.HannaSherry, ManosabaMenuID.HannaSherry},
            { ManosabaMenuID.YukiMeruru, ManosabaMenuID.YukiMeruru },
            { ManosabaMenuID.Margo, ManosabaMenuID.Margo },
            { ManosabaMenuID.Tlipoca1, ManosabaMenuID.Tlipoca1 },
            { ManosabaMenuID.Tlipoca2, ManosabaMenuID.Tlipoca2 }
        };

        public static Texture2D GetBackgroundOnNeed()
        {
            //这里需要给一个允许更改menu的标记
            //不允许更改menu的时候直接返回默认的text值
            //最主要是为了避免不断地复写json文件导致的性能问题
            if(!CanChangeMenu)
                return UseTex;
            //读取这个Mod文件名，看看是否存在我们需要的名字
            //如果不存在，其余状态下默认返回艾玛的背景。
            CanChangeMenu = false;
            CheckSteamUser();
            int targetID = _menuIDMap.TryGetValue(CurrentBackgroundID, out var id) ? id : ManosabaMenuID.Ema;
            ManosabaMenuSystem.Instance.ReimplementJson(targetID);
            return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
        }
        public static void CheckSteamUser()
        {
            //确定一下Steam玩家的状态
            //只有正确的steam名或者正确的steamID，才会允许使用特莉波卡的背景
            ulong steamID = SteamUser.GetSteamID().m_SteamID;
            string steamName = SteamFriends.GetPersonaName().ToLower();
            bool isLegalCondition = steamID == AuthorSteamID || steamName.Contains("kino") || steamName.Contains("冰语");
            if (!isLegalCondition)
            {
                if (CurrentBackgroundID > ManosabaMenuID.Margo)
                    CurrentBackgroundID = ManosabaMenuID.Ema;
            }

        }
        public static void NextMenuOrLastMenuIDGetter(out Texture2D lastMenu, out Texture2D nextMenu)
        {
            //这里的逻辑处理比较奇怪
            //如果返回的id在对应的表单里不存在的话，这里的处理实际上是：LastMenuID会播放最后一张，而nextMenu则切换回艾玛的
            int lastMenuID = _menuIDMap.TryGetValue(CurrentBackgroundID - 1, out var id) ? id : ManosabaMenuID.Margo;
            int nextMenuID = _menuIDMap.TryGetValue(CurrentBackgroundID + 1, out var id2) ? id2 : ManosabaMenuID.Ema;
            nextMenu = ManosabaMenuAssets.ManosabaBackgroundList[nextMenuID].Texture.Value;
            lastMenu = ManosabaMenuAssets.ManosabaBackgroundList[lastMenuID].Texture.Value;
        }
        public static void DrawBackgound()
        {
            Texture2D backGround = UseTex;
            NextMenuOrLastMenuIDGetter(out Texture2D lastMenu, out Texture2D nextMenu);
            //这里会引用到update的更新
            //需要手动放缩背景的贴图大小。
            
            float scaleRatios;
            float targetValue = 0.491f;
            float beginValue = 0.54f;
            if(CurrentBackgroundID > ManosabaMenuID.Margo)
            {
                targetValue = 0.85f;
                beginValue = 1.2f;
            }
            scaleRatios = Clamp(Lerp(beginValue, targetValue, TheScaleRatios), targetValue, beginValue);
            //背景切换。
            //事实上背景会尝试预制在左右两侧。
             
            SB.Draw(backGround, ManosabaMethods.ScreenCenter(), null, Color.White, 0, backGround.Size() / 2, scaleRatios, 0, 0);
            Rectangle rectangle = new(0, 0, Main.screenWidth, Main.screenHeight);
            Texture2D mask = ManosabaMenuAssets.Main_Mask.Texture.Value;
            Main.spriteBatch.Draw(mask, rectangle, Color.White * Logo.LogoScaleRatios);
        }
    }
}

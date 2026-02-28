using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Class;
using SakurabaEmaMod.Menus.Managemments;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class ManosabaBackground
    {
        public static float DrawTimer = 0;
        public static float Progress = 0;
        public static ManosabaHud LoadGame => ManoHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManoHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManoHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManoHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud SwitchMenu => ManoHudManager.UICollection[GetInstance<SwitchMenu>().Type];
        static SpriteBatch SB { get => Main.spriteBatch; }
        /// <summary>
        /// 默认艾玛背景
        /// </summary>
        public static Texture2D UseTex = ManosabaMenuAssets.Still_Ema.Texture.Value;
        public static bool CanChangeMenu = false;
        public static float TheScaleRatios = 0f;
        public static float LogoScaleRatios = 0f;
        public static float BackgroundFading = 0f;
        public static void Update()
        {
            UseTex = GetBackgroundOnNeed();
            BackgroundFading -= 1 / (float)GetSeconds(1);
            BackgroundFading = Clamp(BackgroundFading,0,1);
            //下面的更新值不会再继续做更新了。
            //满足之后
            TheScaleRatios += 1 / (float)GetSeconds(1);
            float scaleRatios = Clamp(Lerp(1.05f, 1f, TheScaleRatios), 1f, 1.05f);

            if(scaleRatios == 1f)
            {
                LogoScaleRatios += 1 / (float)GetSeconds(1);
            }
        }
        public static int CurrentBackgroundID = ManosabaMenuID.Ema;
        public static Texture2D GetBackgroundOnNeed()
        {
            //这里需要给一个允许更改menu的标记
            //不允许更改menu的时候直接返回默认的text值
            //最主要是为了避免不断地复写json文件导致的性能问题
            if(!CanChangeMenu)
            {
                return UseTex; 
            }
            //读取这个Mod文件名，看看是否存在我们需要的名字
            //如果不存在，其余状态下默认返回艾玛的背景。
            CanChangeMenu = false;
            switch (CurrentBackgroundID)
            {
                case ManosabaMenuID.Hiro:
                    //修改当前的文件名，如果存在的话
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.Hiro);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                case ManosabaMenuID.Anan:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.Anan);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                case ManosabaMenuID.Noa:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.Noa);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                case ManosabaMenuID.HannaSherry:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.HannaSherry);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                case ManosabaMenuID.YukiMeruru:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.YukiMeruru);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                case ManosabaMenuID.Margo:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.Margo);
                    return ManosabaMenuAssets.ManosabaBackgroundList[CurrentBackgroundID].Texture.Value;
                default:
                    ManosabaMenuSystem.Instance.ReimplementJson(ManosabaMenuID.Ema);
                    return ManosabaMenuAssets.ManosabaBackgroundList[ManosabaMenuID.Ema].Texture.Value;
            }
        }
        public static void DrawBackgound()
        {
            Texture2D backGround = UseTex;
            Texture2D logo = ManosabaMenuAssets.Main_Title.Texture.Value;
            //这里会引用到update的更新
            //需要手动放缩背景的贴图大小。
            float scaleRatios;
            scaleRatios = Clamp(Lerp(0.47f, 0.44f, TheScaleRatios), 0.44f, 0.47f);
            //背景切换。
            if (BackgroundFading > 0.02f)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * BackgroundFading);
                ManosabaMethods.EnterHudArea(BlendState.NonPremultiplied, SamplerState.LinearClamp);
            }

            SB.Draw(backGround, ManosabaMethods.ScreenCenter(), null, Color.White, 0, backGround.Size() / 2, scaleRatios, 0, 0);
            Vector2 logoPos = new Vector2(Main.screenWidth - 285f, 200f);
            float colorRatios = Clamp(LogoScaleRatios, 0f, 1f);

            SB.Draw(logo, logoPos, null, Color.White * colorRatios, 0, logo.Size() / 2, 0.52f, 0, 0);
            Rectangle rectangle = new(0, 0, Main.screenWidth, Main.screenHeight);
            Texture2D mask = ManosabaMenuAssets.Main_Mask.Texture.Value;
            Main.spriteBatch.Draw(mask, rectangle, Color.White * colorRatios);

        }
    }
}

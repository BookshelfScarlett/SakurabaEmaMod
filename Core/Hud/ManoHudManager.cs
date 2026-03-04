using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core.Hud
{
    public class ManoHudManager : ModSystem
    {
        // 所有UI的集合，UI的Type为在集合中的索引
        public static List<ManosabaHud> UICollection = new List<ManosabaHud>();
        public static int MaxDepth = 21;
        public static bool[] ActiveDepth = new bool[MaxDepth];
        public static int[] ActiveDepthCount = new int[MaxDepth];
        public static int BlockAllUI;
        public override void Load()
        {
            On_Main.UpdateUIStates += PostUpdateUI;
        }
        public override void Unload()
        {
            On_Main.UpdateUIStates -= PostUpdateUI;
        }
        public static void PostUpdateUI(On_Main.orig_UpdateUIStates orig, GameTime gameTime)
        {
            orig(gameTime);
            for (int i = 0; i < ActiveDepth.Length; i++)
            {
                if (ActiveDepthCount[i] == 0)
                    ActiveDepth[i] = false;
                else
                    ActiveDepth[i] = true;
            }
            for (int i = 0; i < ActiveDepthCount.Length; i++)
            {
                if (ActiveDepthCount[i] > 0)
                    ActiveDepthCount[i]--;
            }
            if (BlockAllUI > 0)
                BlockAllUI--;
        }
    }
}

using log4net.Core;
using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Players;
using SakurabaEmaMod.Items;
using System;
using System.Data;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Methods
{
    public static partial class ManosabaMethods
    {
        public static ManosabaPlayer ManosabaMod(this Player player) => player.GetModPlayer<ManosabaPlayer>();
        /// <summary>
        /// 玩家是否站立不动，处理方式为计算玩家水平速度模+垂直速度模的总和
        /// </summary>
        /// <param name="player"></param>
        /// <param name="standingStillThreshold">判断站立不动的最低速度区间，默认值取0.5f</param>
        /// <returns></returns>
        public static bool IsStandingStil(this Player player, float standingStillThreshold = 0.5f)
        {
            return (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y)) < standingStillThreshold;
        }
        public static bool HasProj<T>(this Player player) where T : ModItem => HasProj(player, ItemType<T>());
        public static bool HasProj(this Player player, int projID) => player.ownedProjectileCounts[projID] > 0;
        //public static Vector2 ScreenCenter
        //{
        //    get
        //    {
        //        return new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
        //    }
        //}

    }
}

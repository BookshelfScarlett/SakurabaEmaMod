using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public abstract class CharactorProj : ModProjectile, ILocalizedModType
    {
        public Player Owner => Main.player[Projectile.owner];
        /// <summary>
        /// 标记这个射弹归属的人物
        /// 一般用于一些专属的射弹，如诺亚的画笔
        /// </summary>
        public virtual short SetCharactor => -1;
        public SpriteBatch SB { get => Main.spriteBatch; }
    }
}

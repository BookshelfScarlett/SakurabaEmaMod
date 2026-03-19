using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public abstract class CharactorProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projs";
        public Player Owner => Main.player[Projectile.owner];
        /// <summary>
        /// 标记这个射弹归属的人物
        /// 一般用于一些专属的射弹，如诺亚的画笔
        /// </summary>
        public virtual short SetCharactor => -1;
        public SpriteBatch SB { get => Main.spriteBatch; }
    }
}

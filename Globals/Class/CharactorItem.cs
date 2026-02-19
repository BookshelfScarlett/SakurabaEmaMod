using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Globals.Enums;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public abstract class CharactorItem : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Item";
        public override string Texture => base.Texture;
        /// <summary>
        /// 当前物品归属的人物
        /// </summary>
        public virtual Charactor SetCharactor { get; }
        /// <summary>
        /// 物品大小
        /// </summary>
        public virtual int Size { get; }
        public sealed override void SetDefaults()
        {
            Item.width = Item.height = Size;
            ExSD();
        }
        public virtual void ExSD()
        {

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
    }
}

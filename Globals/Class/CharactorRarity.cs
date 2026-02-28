using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public abstract class CharactorRarity : ModRarity
    {
        public abstract void DrawItemName(DrawableTooltipLine line);
        public abstract void DrawFlavorName(DrawableTooltipLine line);
        public abstract void DrawFlavorTooltip(DrawableTooltipLine line);
        public abstract void DrawMisc(DrawableTooltipLine line);
    }
}

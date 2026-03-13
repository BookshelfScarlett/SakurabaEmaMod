using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Rarity.RarityShiny;
using Terraria.ID;

namespace SakurabaEmaMod.Items.Vanity
{
    public class TachibanaSherryItem : CharactorVanity
    {
        public override short SetCharactor => ManosabaGirlID.TachibanaSherry;
        public override int SetRarity => RarityType<TachibanaSherryRarity>();
        public override int Size => 32;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Lens, 5).
                AddIngredient(ItemID.Silk).
                DisableDecraft().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}

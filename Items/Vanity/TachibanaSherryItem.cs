using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Rarity.RarityShiny;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Items.Vanity
{
    public class TachibanaSherryItem : CharactorVanity
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override short SetCharactor => ManosabaGirlID.TachibanaSherry;
        public override int SetRarity => RarityType<TachibanaSherryRarity>();
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

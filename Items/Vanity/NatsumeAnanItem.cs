using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Items.Vanity
{
    public class NatsumeAnanItem : CharactorVanity
    {
        public override short SetCharactor => ManosabaGirlID.NatsumeAnan;
        public override void ExSD()
        {
            Item.width = 28;
            Item.height = 32;
        }
        public override void ExLoad()
        {
            EquipLoader.AddEquipTexture(Mod, TexturePath + "Hair", EquipType.Back, this);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Book).
                AddIngredient(ItemID.Silk, 5).
                AddIngredient(ItemID.Feather, 2).
                AddTile(TileID.Beds).
                DisableDecraft().
                Register();
        }
    }
}

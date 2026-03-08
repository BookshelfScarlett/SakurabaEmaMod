using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Rarity.RarityShiny;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SakurabaEmaMod.Items.Vanity
{
    public class JougasakiNoaItem : CharactorVanity
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override short SetCharactor => ManosabaGirlID.JougasakiNoa;
        public override int RaritySet => RarityType<JougasakiNoaRarity>();
        public override int Size => 32;
        public override void ExLoad()
        {
            EquipLoader.AddEquipTexture(Mod, TexturePath + "Hair", EquipType.Back, this);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.Wood, 15).
                AddIngredient(ItemID.FallenStar, 1).
                AddIngredient(ItemID.WaterBucket).
                AddTile(TileID.DyeVat).
                Register();
        }
    }
    public class Noalayer : ModPlayer
    {
        public bool Equipped = false;
        public override void ResetEffects()
        {
            Equipped = false;
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(Equipped), Equipped);
        }
        public override void LoadData(TagCompound tag)
        {
            Equipped = tag.GetBool(nameof(Equipped));
        }
        public override void FrameEffects()
        {
            bool equip = false;
            if (Main.gameMenu)
            {
                GetArmorSlotItem(ref equip);
            }
            if (equip)
            {
                string name = nameof(JougasakiNoaItem);
                Player.back = EquipLoader.GetEquipSlot(Mod, name, EquipType.Back);
                Player.legs = EquipLoader.GetEquipSlot(Mod, name, EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, name, EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, name, EquipType.Head);

            }
        }
        public void GetArmorSlotItem(ref bool equip)
        {
            foreach (Item item in Player.armor)
            {
                if (item.type == ItemType<NikaidouHiroItem>())
                {
                    equip = true;
                    break;
                }
            }
        }
    }
}

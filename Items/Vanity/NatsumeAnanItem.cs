using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
    public class AnanPlayer : ModPlayer
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
            if(Main.gameMenu)
            {
                GetArmorSlotItem(ref equip);
            }
            if(equip)
            {
                string name = "NatsumeAnanItem";
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
                if (item.type == ItemType<NatsumeAnanItem>())
                {
                    equip = true;
                    break;
                }
            }
        }
    }
}

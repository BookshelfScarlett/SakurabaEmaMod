using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Rarity.RarityShiny;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SakurabaEmaMod.Items.Vanity
{
    public class TachibanaSherryItem : CharactorVanity
    {
        public override short SetCharactor => ManosabaGirlID.TachibanaSherry;
        public override TextboxVanity VanityData => new TextboxVanity()
        {
            BackgroundEdgeColor = Color.White,
            BackgroundColor = Color.CornflowerBlue* .35f,
            TextColor = Color.White,
            TextEdgeColor = Color.Lerp(Color.RoyalBlue, Color.CornflowerBlue, 0.4f),
            TitleEdgeColor = Color.Lerp(Color.RoyalBlue, Color.CornflowerBlue, 0.4f),
            TitleColor = Color.White,
        };

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
    public class SherryPlayer: ModPlayer
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
                string name = "TachibanaSherryItem";
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
                if (item.type == ItemType<TachibanaSherryItem>())
                {
                    equip = true;
                    break;
                }
            }
        }
    }

}

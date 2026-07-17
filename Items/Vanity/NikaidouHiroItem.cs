using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SakurabaEmaMod.Items.Vanity
{
    internal class NikaidouHiroItem : CharactorVanity
    {
        public override short SetCharactor => ManosabaGirlID.NikaidouHiro;
        public override TextboxVanity VanityData => new TextboxVanity()
        {
            BackgroundEdgeColor = Color.Lerp(Color.Red, Color.IndianRed,0.1f) with { A = 255},
            BackgroundColor = Color.Lerp(Color.Black,Color.White,0.132f)* .5f,
            TextColor = Color.Lerp(Color.Red,Color.White,0.134f) with { A = 255},
            TextEdgeColor = Color.Black,
            TitleEdgeColor = Color.Red,
            TitleColor = Color.Black,
        };

        public override void ExLoad()
        {
            EquipLoader.AddEquipTexture(Mod, TexturePath + "Hair", EquipType.Back, this);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.JungleRose).
                AddIngredient(ItemID.Silk, 5).
                AddTile(TileID.Loom).
                DisableDecraft().
                Register();
        }
    }

    public class HiroPlayer: ModPlayer
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
                string name = "NikaidouHiroItem";
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

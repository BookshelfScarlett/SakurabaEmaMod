using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityShiny;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.BackupIO;

namespace SakurabaEmaMod.Items.Vanity
{
    public class JougasakiNoahItem : CharactorVanity
    {
        public override short SetCharactor => ManosabaGirlID.JougasakiNoah;
        public override int SetRarity => RarityType<JougasakiNoahRarity>();
        public override TextboxVanity VanityData => new TextboxVanity()
        {
            BackgroundEdgeColor = Color.White * .95f,
            BackgroundColor = Color.Black * .35f,
            TextColor = Color.White,
            TextEdgeColor = Color.Black,
            TitleEdgeColor = Color.White,
            TitleColor = Color.Black,
        };
        public override void ExLoad()
        {
            EquipLoader.AddEquipTexture(Mod, TexturePath + "Hair", EquipType.Back, this);
        }
        public override void ExModifyTooltip(List<TooltipLine> tooltips)
        {
            //i诺TV你们赢了
            string localName = Main.LocalPlayer.ManosabaMod().NoahCryDeath ? "CrySound" : "RegularSound";
            tooltips.CreateTooltip(this.GetLocalizedValue(localName), LineName: "SoundName");
        }
        public override bool ExPreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {

            if (line.IsThisLine("Tooltip0"))
            {
                JougasakiNoahRarity.DrawMore(line);
                return false;
            }
            if (line.Name == "SoundName" && line.Mod == Mod.Name)
            {
                JougasakiNoahRarity.DrawMore(line);
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.Wood, 15).
                AddIngredient(ItemID.FallenStar, 1).
                AddIngredient(ItemID.WaterBucket).
                DisableDecraft().
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
                string name = nameof(JougasakiNoahItem);
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
                if (item.type == ItemType<JougasakiNoahItem>())
                {
                    equip = true;
                    break;
                }
            }
        }
    }
}

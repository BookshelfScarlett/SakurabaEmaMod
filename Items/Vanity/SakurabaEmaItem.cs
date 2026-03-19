using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityShiny;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Items.Vanity
{
    /// <summary>
    /// 代办：将其扔到统一基类管理
    /// </summary>
    public class SakurabaEmma : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items";
        public static string ItemPath => "SakurabaEmaMod/Assets/Texture/CharactorSets/SakurabaEma/";
        public override string Texture => $"{ItemPath}Item";
        //没有理由给这个东西敲词缀，说实话
        public override bool AllowPrefix(int pre) => false;
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Head", EquipType.Head, this);
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Body", EquipType.Body, this);
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Legs", EquipType.Legs, this);
        }
        public override void SetStaticDefaults()
        {

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }
        public override bool ConsumeItem(Player player) => false;
        public int Timer = 0;
        public int FlavorTooltipIndex = 0;
        public int RealTooltipIndex = 0;
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = RarityType<SakurabaEmaRarity>();
            Item.vanity = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 15).
                DisableDecraft().
                AddTile(TileID.Loom).
                Register();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //遍历一遍寻找需要绘制的特殊台词位置的索引
            //这里需要把对应的原罪名与原罪文本直接植入到物品名的下方，让其看起来比较协调
            FlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            //通过本地化路径获取相应的内容，一个是原罪名，第二个为原罪文本
            string value = this.GetLocalizedValue("FlavorTooltip").ToLangValue();
            string realTooltipValue = this.GetLocalizedValue("RealTooltip").ToLangValue();
            //实例化TooltipLine，这里的名字不能乱写，需要作为后面绘制特殊效果用的一个索引
            TooltipLine flavorTooltip = new TooltipLine(Mod, "FlavorTooltipName", value);
            TooltipLine realTooltip = new TooltipLine(Mod, "RealToolTipName", realTooltipValue);
            //植入Tooltip。
            tooltips.Insert(FlavorTooltipIndex + 1, flavorTooltip);
            tooltips.Insert(FlavorTooltipIndex + 2, realTooltip);
            //艾玛有中键音效切换，这里需要提示玩家当前使用的版本
            //直接用的封装CreateTooltip方法
            Player player = Main.LocalPlayer;
            if (player.ManosabaMod().EmaKiangSound)
            {
                tooltips.CreateTooltip(this.GetLocalizedValue("KiangSound"), LineName: "SoundName");
            }
            else
                tooltips.CreateTooltip(this.GetLocalizedValue("RegularSound"), LineName: "SoundName");
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            //为物品名本身绘制特效
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                SakurabaEmaRarity.DrawRarity(line);
                return false;
            }
            //为原罪名文本绘制特效
            if (line.Mod == Mod.Name && line.Name == "FlavorTooltipName")
            {
                SakurabaEmaRarity.DrawFlavor(line);
                return false;
            }
            //为原罪文本绘制特效
            if (line.Mod == Mod.Name && line.Name == "RealToolTipName")
            {
                SakurabaEmaRarity.DrawTooltip(line);
                return false;
            }
            if (line.IsThisLine("Vanity") || line.IsThisLine("Equipable"))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;
            }
            if (line.IsThisLine("Tooltip0"))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;

            }
            if (line.IsThisLine("SoundName", Mod.Name))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;
            }
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            base.PostDrawTooltipLine(line);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = TextureAssets.Item[Type].Value;
            Vector2 position = Item.position - Main.screenPosition + tex.Size() / 2;
            Rectangle iFrame = tex.Frame();
            //绘制物品时装描边
            for (int i = 0; i < 16; i++)
                spriteBatch.Draw(tex, position + ToRadians(i * 60f).ToRotationVector2() * 2.4f, null, Color.Pink with { A = 0 }, 0f, tex.Size() / 2, scale, 0, 0f);
            //绘制物品本身
            spriteBatch.Draw(tex, position, iFrame, Color.White, 0f, tex.Size() / 2, scale, 0f, 0f);
            Lighting.AddLight(position, TorchID.UltraBright);
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = TextureAssets.Item[Type].Value;
            //描边。
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(tex, position + ToRadians(60f * i).ToRotationVector2() * 2.1f, frame, Color.LightPink.ToAddColor(), 0f, origin, scale, 0, 0);
            }
            //本身
            spriteBatch.Draw(tex, position, frame, Color.White, 0, origin, scale, 0, 0);
            return false;
        }
        public override void UpdateVanity(Player player)
        {
            player.ManosabaMod().ManosabaGirl = ManosabaGirlID.SakurabaEma;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.ManosabaMod().ManosabaGirl = ManosabaGirlID.SakurabaEma;
        }
    }
}

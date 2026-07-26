using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Globals.Avator;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Globals.Textbox;
using SakurabaEmaMod.Rarity.RarityShiny;
using System.Collections.Generic;
using System.Security;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
        public Texture2D AvatorTexture => Request<Texture2D>(ItemPath + "Avatar").Value;
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
        public static IReadOnlyList<TooltipLine> CacheList = [];
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //遍历一遍寻找需要绘制的特殊台词位置的索引
            //这里需要把对应的原罪名与原罪文本直接植入到物品名的下方，让其看起来比较协调
            if (ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
            {
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
                
            }
                //艾玛有中键音效切换，这里需要提示玩家当前使用的版本
                //直接用的封装CreateTooltip方法
            Player player = Main.LocalPlayer;
            if (player.ManosabaMod().EmaKiangSound)
            {
                tooltips.CreateTooltip(this.GetLocalizedValue("KiangSound"), LineName: "SoundName");
            }
            else
                tooltips.CreateTooltip(this.GetLocalizedValue("RegularSound"), LineName: "SoundName");
            CacheList = tooltips;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            //为物品名本身绘制特效
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                SakurabaEmaRarity.DrawRarity(line);
                TextboxManager.FirstLineY = line.Y;
                return false;
            }
            if (ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
            {
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
            float height = 0;
            if (!ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
            {
                string titleString = this.GetLocalizationKey("FlavorTooltip").ToLangValue();
                string sinString = this.GetLocalizedValue("RealTooltip").ToLangValue();
                TextboxSettings sets = new TextboxSettings()
                {
                    TitleText = titleString,
                    TitleEdgeColor = Color.White,
                    TitleTextColor = Color.HotPink,
                    HasTitle = true,
                    BackgroundColor = Color.Lerp(Color.WhiteSmoke, Color.HotPink, 0.50f) * .46f,
                    BackgroundEdgeColor = Color.White * .98f,
                    MainText = sinString,
                    TextColor = Color.White,
                    TextEdgeColor = Color.HotPink,
                    TitleTextSize = 1.15f
                };
                height = TextboxMethods.DrawTextboxTooltipWithBackground(line, CacheList, ref sets);
            }
            if (ManosabaClientConfig.Instance.NoCharactorAvatar)
                return;
            AvatorSettings avatorSettings = new AvatorSettings()
            {
                BackgroundColor = Color.Lerp(Color.WhiteSmoke, Color.HotPink, 0.50f) * .46f,
                BackgroundEdgeColor = Color.White * .98f,
                AvatorTexture = AvatorTexture,
                Scale = 1f
            };
            if (height != 0)
                height += 30;
            AvatorMethods.DrawAvatorWithBackground(line, CacheList, ref  avatorSettings, height);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Type, out Texture2D itemTexture, out Rectangle itemFrame);
            Vector2 drawOrigin = itemFrame.Size() / 2;
            Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
            spriteBatch.Draw(itemTexture, drawPosition, itemFrame, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0);
        }
        public override void PostUpdate()
        {
            if(!ManosabaClientConfig.Instance.ParticleDontEmitLight)
            Lighting.AddLight(Item.Center, TorchID.White);
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
    public class EmaPlayer: ModPlayer
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
                string name = "SakurabaEmma";
                Player.legs = EquipLoader.GetEquipSlot(Mod, name, EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, name, EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, name, EquipType.Head);

            }
        }
        public void GetArmorSlotItem(ref bool equip)
        {
            foreach (Item item in Player.armor)
            {
                if (item.type == ItemType<SakurabaEmma>())
                {
                    equip = true;
                    break;
                }
            }
        }
    }
}

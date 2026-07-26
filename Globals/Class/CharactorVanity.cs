using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Core;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Globals.Avator;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Globals.Textbox;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public struct TextboxVanity(Color textColor, Color titleColor, Color textEdgeColor, Color titleEdgeColor, Color backgroundColor, Color backgroundEdgeColor)
    {
        public Color TextColor = textColor;
        public Color TitleColor = titleColor;
        public Color TextEdgeColor = textEdgeColor;
        public Color TitleEdgeColor = titleEdgeColor;
        public Color BackgroundColor = backgroundColor;
        public Color BackgroundEdgeColor = backgroundEdgeColor;
    }
    public abstract class CharactorVanity : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Items";
        /// <summary>
        /// 当前物品归属的人物
        /// 复写这个同样会尝试去寻找对应的材质。
        /// </summary>
        public virtual short SetCharactor { get; }
        /// <summary>
        /// 物品大小
        /// </summary>
        public virtual int Size => 24;
        /// <summary>
        /// 物品使用的稀有度
        /// </summary>
        public virtual int SetRarity => ManosabaRaritySystem.Instance.SetRarityType(SetCharactor);
        /// <summary>
        /// 用于绘制物品的描边颜色
        /// 在世界范围和物品栏内使用
        /// </summary>
        public virtual Color EdgeColor => Color.White;
        public virtual bool IsVanityItem => true;
        public int FlavorTooltipIndex = -1;
        public string TexturePath => $"SakurabaEmaMod/Assets/Texture/CharactorSets/{GetName}/";
        public Texture2D AvatorTexture => Request<Texture2D>(TexturePath + "Avatar").Value;
        public virtual TextboxVanity VanityData { get; set; }
        private string GetName
        {
            get
            {
                return SetCharactor switch
                {
                    ManosabaGirlID.SakurabaEma => nameof(Charactor.SakurabaEma),
                    ManosabaGirlID.NikaidouHiro => nameof(Charactor.NikaidouHiro),
                    ManosabaGirlID.NatsumeAnan => nameof(Charactor.NatsumeAnan),
                    ManosabaGirlID.JougasakiNoah => nameof(Charactor.JougasakiNoah),
                    ManosabaGirlID.TachibanaSherry => nameof(Charactor.TachibanaSherry),
                    ManosabaGirlID.ToonoHanna => nameof(Charactor.ToonoHanna),
                    _ => nameof(Charactor.None),
                };
            }
        }
        public override string Texture => $"{TexturePath}Item";
        public sealed override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.rare = SetRarity;
            Item.vanity = true;
            Item.accessory = true;
            ExSD();
        }
        public sealed override void SetStaticDefaults()
        {

            if (Main.netMode == NetmodeID.Server)
                return;

            if (IsVanityItem)
            {
                int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

                ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
                ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
                ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
            }
            ExSSD();

        }
        public virtual void ExSSD() { }
        public sealed override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            if (IsVanityItem)
            {
                EquipLoader.AddEquipTexture(Mod, TexturePath + "Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, TexturePath + "Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, TexturePath + "Legs", EquipType.Legs, this);
            }
            ExLoad();
        }
        /// <summary>
        /// 额外的加载
        /// 注意的是，原本的Load钩子已经被封存，而且通过类的基础存放了盔甲部件的三个东西
        /// 如果当前人物有其他的需求，请复写该钩子
        /// </summary>
        public virtual void ExLoad() { }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //标记当前使用的人物Enum
            //如果hideVisual的情况下不使用，然后他会自动在ResetEffect尝试重置为none。
            if (!hideVisual)
            {
                player.ManosabaMod().ManosabaGirl = SetCharactor;
                ExtraUpdate(player);
            }
        }
        public override void UpdateVanity(Player player)
        {
            player.ManosabaMod().ManosabaGirl = SetCharactor;
            ExtraUpdate(player);
        }
        public virtual void ExtraUpdate(Player player) { }
        public virtual void ExSD()
        {

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //遍历一遍寻找需要绘制的特殊台词位置的索引
            //这里需要把对应的原罪名与原罪文本直接植入到物品名的下方，让其看起来比较协调
            if (ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
            {
                FlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
                //通过本地化路径获取相应的内容，一个是原罪名，第二个为原罪文本
                string value = ManosabaMethods.ToLangValue(this.GetLocalizedValue("SinnerType"));
                string realTooltipValue = ManosabaMethods.ToLangValue(this.GetLocalizedValue("SinnerTooltip"));
                //实例化TooltipLine，这里的名字不能乱写，需要作为后面绘制特殊效果用的一个索引
                TooltipLine flavorTooltip = new TooltipLine(Mod, "SinnerTypeName", value);
                TooltipLine realTooltip = new TooltipLine(Mod, "SinnerTooltipName", realTooltipValue);
                //植入Tooltip。
                tooltips.Insert(FlavorTooltipIndex + 1, flavorTooltip);
                tooltips.Insert(FlavorTooltipIndex + 2, realTooltip);
            }
                CacheTooltipLine = tooltips;
            ExModifyTooltip(tooltips);
        }
        public virtual void ExModifyTooltip(List<TooltipLine> tooltips) { }
        /// <summary>
        /// 原有的PreDrawTooltipLine钩子已经被封存无法复写
        /// 如果真的需要完全自定义，直接复写这个钩子并返回真，阻止这个类的原本预制的绘制
        /// </summary>
        /// <param name="line"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        public virtual bool CustomDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) => false;
        public static IReadOnlyList<TooltipLine> CacheTooltipLine = [];
        public sealed override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.IsItemName())
            {
                TextboxManager.FirstLineY = line.Y;
            }
            if (CustomDrawTooltipLine(line, ref yOffset))
                return true;
            //这里的代码已经被高度简化了，最主要是为了实现无需重复批量创建的效果
            //如果需要去查阅的话，最好直接跳转到对应的System下面查看
            if (ManosabaRaritySystem.Instance.DrawRarityEffect(line, SetCharactor))
            {
                if (ExPreDrawTooltipLine(line, ref yOffset))
                    return base.PreDrawTooltipLine(line, ref yOffset);
                else
                    return false;
            }
            else
                return false;
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
        public virtual bool ExPreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) => true;
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            float height = 0;
            if (!ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
            {
                string titleString = this.GetLocalizationKey("SinnerType").ToLangValue();
                string sinString = this.GetLocalizedValue("SinnerTooltip").ToLangValue();
                TextboxSettings sets = new TextboxSettings()
                {
                    BackgroundColor = VanityData.BackgroundColor,
                    BackgroundEdgeColor = VanityData.BackgroundEdgeColor,
                    HasTitle = true,
                    TextColor = VanityData.TextColor,
                    TextEdgeColor = VanityData.TextEdgeColor,
                    TitleEdgeColor = VanityData.TitleEdgeColor,
                    TitleTextColor = VanityData.TitleColor,
                    TitleText = titleString,
                    MainText = sinString,
                    TitleTextSize = 1.15f
                };
                height = TextboxMethods.DrawTextboxTooltipWithBackground(line, CacheTooltipLine, ref sets);
            }
            if (ManosabaClientConfig.Instance.NoCharactorAvatar)
                return;
            AvatorSettings avatorSettings = new AvatorSettings()
            {
                BackgroundColor = VanityData.BackgroundColor,
                BackgroundEdgeColor = VanityData.BackgroundEdgeColor,
                AvatorTexture = AvatorTexture,
                Scale = 1f
            };
            if (height != 0)
                height += 30;
            AvatorMethods.DrawAvatorWithBackground(line, CacheTooltipLine, ref  avatorSettings, height);
        }
    }
}

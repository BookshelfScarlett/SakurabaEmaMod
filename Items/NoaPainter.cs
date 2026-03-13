using Microsoft.Xna.Framework;
using SakurabaEmaMod.Core;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Cutscenes;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Projs;
using SakurabaEmaMod.Rarity.RarityShiny;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Items
{
    public class NoaPainter : CharactorItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override short SetCharactor => ManosabaGirlID.JougasakiNoah;
        public override int Size => 44;
        public override string Texture => GetAsset("Items");
        public override bool AllowPrefix(int pre) => false;
        public int FlavorTooltipIndex = -1;
        public override void ExSD()
        {
            Item.rare = SetRarity;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<NoaPainterProj>();
            Item.shootSpeed = 12f;
            Item.channel = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //遍历一遍寻找需要绘制的特殊台词位置的索引
            //这里需要把对应的原罪名与原罪文本直接植入到物品名的下方，让其看起来比较协调
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
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (!ManosabaRaritySystem.Instance.DrawRarityEffect(line, SetCharactor))
            {
                if (line.IsThisLine("Tooltip0") || line.IsThisLine("Tootlip1"))
                {
                    JougasakiNoahRarity.DrawMore(line);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool? UseItem(Player player)
        {
                player.ManosabaMod().IsDoneFinalBossFight = false;
            if (player.itemAnimation == player.itemAnimationMax)
            {
                CutsceneManager.QueueCutscene(GetInstance<BloomCutScene>());
            }
            return base.UseItem(player);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }
        public override bool CanUseItem(Player player) => !player.HasProj(Item.shoot);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            proj.owner = player.whoAmI;
            proj.Center = player.Center + new Vector2(28f, 0f).RotatedBy(velocity.ToRotation()); 
        }
    }
}

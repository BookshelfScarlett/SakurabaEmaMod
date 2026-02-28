using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Rarity.RarityShiny
{
    public class JougasakiNoaRarity :CharactorRarity
    {
        public static List<RaritySparkle> RaritySparkles = [];
        public static List<RaritySparkle> FlavorSparkles = [];

        public override Color RarityColor => Color.Lerp(Color.White, Color.Silver, 0.9f);
        public override void DrawItemName(DrawableTooltipLine line)
        {
            //绘制需要的……发光背景。
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Silver, Color.Black, Color.Lerp(Color.White, Color.Silver, 0f), 0.8f);
            PostDrawRarity(ref RaritySparkles, line);

        }
        public override void DrawFlavorName(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Silver, Color.Black, Color.Lerp(Color.White, Color.Silver, 0f), 0.95f);
            PostDrawFlavor(ref FlavorSparkles, line);
        }
        public override void DrawFlavorTooltip(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Black, Color.White);
        }
        public override void DrawMisc(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Black, Color.White);
        }
        public static void DrawMore(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Black, Color.White);
        }


        public static void PostDrawFlavor(ref List<RaritySparkle> particleList, DrawableTooltipLine tooltipLine)
        {
            Vector2 textSize = tooltipLine.Font.MeasureString(tooltipLine.Text);
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if(Main.rand.NextBool(12))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f) * 0.9f;
                int lifetime = 80;
                Vector2 position = Main.rand.NextVector2FromRectangle(new(-(int)(textSize.X * 0.50f), -(int)(textSize.Y * 0.6f), (int)(textSize.X * 0.8f), (int)(textSize.Y * 0.75f)));
                Vector2 velocity = Vector2.UnitX * Main.rand.NextFloat(0.5f, 1.15f) * 0.41f;
                RarityShinyOrb rarityShinyOrb = new RarityShinyOrb(position, velocity, RandLerpColor(Color.White, Color.Silver).ToAddColor(), lifetime, scale);
                particleList.Add(rarityShinyOrb);
            }
            //最后在通用的方法内进行更新
            RarityDrawHelper.UpdateTooltipParticles(tooltipLine, ref particleList);
        }
        public static void PostDrawRarity(ref List<RaritySparkle> particleList, DrawableTooltipLine tooltipLine)
        {
            Vector2 textSize = tooltipLine.Font.MeasureString(tooltipLine.Text);
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(10))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f) * 0.8f;
                int lifetime = 80;
                Vector2 position = Main.rand.NextVector2FromRectangle(new(-(int)(textSize.X * 0.5f), -(int)(textSize.Y * 0.7f), (int)textSize.X, (int)(textSize.Y * 0.35f)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.5f, 1.15f) * 0.41f;
                RarityShinyOrb rarityShinyOrb = new RarityShinyOrb(position, velocity, RandLerpColor(Color.White, Color.Silver).ToAddColor(), lifetime, scale);
                particleList.Add(rarityShinyOrb);

            }
            //最后在通用的方法内进行更新
            RarityDrawHelper.UpdateTooltipParticles(tooltipLine, ref particleList);
        }

    }
}

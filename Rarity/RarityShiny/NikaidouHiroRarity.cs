using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Rarity.RarityShiny
{
    public class NikaidouHiroRarity : CharactorRarity
    {
        public static List<RaritySparkle> RaritySparkles = [];
        public static List<RaritySparkle> FlavorSparkles = [];

        public override Color RarityColor => Color.Crimson;
        public override void DrawItemName(DrawableTooltipLine line)
        {
            //绘制需要的……发光背景。
            //DrawGlowBackground(line);
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Red, Color.Lerp(Color.Crimson, Color.Red, 0.4f), Color.Black);
            PostDrawRarity(ref RaritySparkles, line);
        }

        public override void DrawFlavorName(DrawableTooltipLine line)
        {
            PostDrawFlavor(ref FlavorSparkles, line);
            RarityDrawHelper.DrawCustomTooltipLine(line, Color.Red, Color.Lerp(Color.Crimson, Color.Red, 0.4f), Color.Black);
        }
        public override void DrawFlavorTooltip(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line,  Color.Black,Color.Lerp(Color.Crimson, Color.Red, 0.4f));
        }
        public override void DrawMisc(DrawableTooltipLine line)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line,  Color.Black,Color.Lerp(Color.Crimson, Color.Red, 0.4f));
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
                RarityShinyOrb rarityShinyOrb = new RarityShinyOrb(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(), lifetime, scale);
                RarityShinyOrb rarityShinyOrb2 = new RarityShinyOrb(position, velocity, Color.White.ToAddColor(), lifetime, scale * 0.5f);
                particleList.Add(rarityShinyOrb);
                particleList.Add(rarityShinyOrb2);
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
                float scale = Main.rand.NextFloat(0.10f * 0.5f, 0.10f) * 0.8f;
                int lifetime = 80;
                Vector2 position = Main.rand.NextVector2FromRectangle(new(-(int)(textSize.X * 0.5f), -(int)(textSize.Y * 0.3f), (int)textSize.X, (int)(textSize.Y * 0.35f)));
                Vector2 velocity = -Vector2.UnitY  * Main.rand.NextFloat(1.5f, 2.15f);
                SakuraPetals sakuraPetals = new(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(), lifetime, RandRotTwoPi, 1f, scale, 0.1f, true);
                particleList.Add(sakuraPetals);
            }
            //最后在通用的方法内进行更新
            RarityDrawHelper.UpdateTooltipParticles(tooltipLine, ref particleList);
        }

    }
}

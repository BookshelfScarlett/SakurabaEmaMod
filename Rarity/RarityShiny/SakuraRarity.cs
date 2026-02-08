using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Rarity.RarityShiny
{
    public class SakuraRarity : ModRarity
    {
        public static List<RaritySparkle> RaritySparkles = [];
        public override Color RarityColor => Color.Lerp(Color.HotPink, Color.DeepPink, 0.9f);
        public static void DrawRarity(DrawableTooltipLine drawableTooltipLine)
        {
            RarityDrawHelper.DrawCustomTooltipLine(drawableTooltipLine, Color.HotPink, Color.Violet.ToAddColor(), Color.HotPink);
            PostDrawRarity(ref RaritySparkles, drawableTooltipLine);
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
                Vector2 velocity = -Vector2.UnitY * Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(1.5f, 2.15f);
                SakuraPetals sakuraPetals = new SakuraPetals(position, velocity, RandLerpColor(Color.LightPink, Color.HotPink).ToAddColor(), lifetime, RandRotTwoPi, 1f, scale, 0.1f);
                particleList.Add(sakuraPetals);
            }
            //最后在通用的方法内进行更新
            RarityDrawHelper.UpdateTooltipParticles(tooltipLine, ref particleList);
        }
    }
}

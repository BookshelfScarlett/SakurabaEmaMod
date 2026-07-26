using Microsoft.Xna.Framework;
using SakurabaEmaMod.Assets.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Resources
{
    public partial class ResourceBarOverlayer : ModResourceOverlay
    {
        public static float SherryDrawTimer = 0;
        /// <summary>
        /// 在PostDraw内，绘制雪莉的帽子（板子）
        /// <br>其与层在魔力条填充上方</br>
        /// </summary>
        /// <param name="context"></param>
        public void DrawSherryHPRightPanel(ResourceOverlayDrawContext context)
        {
            context.texture = ManosabaResource.TachibanaSherryBar.Panel_HP_Right;
            context.source = context.texture.Frame();
            context.position += Vector2.UnitX * 5f - Vector2.UnitY * 30f;
            context.origin = Vector2.Zero;
            context.scale *= 1f;
            context.rotation -= ToRadians(0);
            context.Draw();
        }

        /// <summary>
        /// 在PostDraw内，绘制雪莉的魔力条装饰（板子）
        /// <br>其与层在魔力条填充上方</br>
        /// </summary>
        /// <param name="context"></param>
        public void DrawSherryManaRightPanel(ResourceOverlayDrawContext context)
        {
            SherryDrawTimer += 1f;
            if (ToRadians(360f) < ToRadians(SherryDrawTimer))
                SherryDrawTimer= -360f;
            Osci = Vector2.UnitY * (float)(Math.Sin(ToRadians(SherryDrawTimer / 2)) * 3f);

            context.texture = ManosabaResource.TachibanaSherryBar.Panel_MP_Right;
            context.source = context.texture.Frame();
            context.position += Vector2.UnitX * 5f + Vector2.UnitY * 5f + Osci * 0.5f;
            context.origin = Vector2.Zero;
            context.scale *= 1.1f;
            context.rotation -= ToRadians(10);
            context.Draw();
        }
    }
}

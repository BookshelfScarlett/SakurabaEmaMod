using Microsoft.Xna.Framework;
using SakurabaEmaMod.Assets.Register;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Resources
{
    public partial class ResourceBarOverlayer : ModResourceOverlay
    {
        public static float NoaDrawTimer = 0f;
        public static Vector2 Osci;
        /// <summary>
        /// 在predraw内，绘制诺亚的血条装饰（板子）
        /// 由于图层问题，这里必须得在魔力条的图层画以便覆盖血条填充图层
        /// </summary>
        /// <param name="context"></param>
        public void DrawNoahLifeRightPanel(ResourceOverlayDrawContext context)
        {
            //由于图层问题，诺亚的血条板子必须得在这里画
            context.texture = ManosabaResource.JougasakiNoahBar.Panel_HP_Right;
            context.source = context.texture.Frame();
            context.position += Vector2.UnitX * 15f - Vector2.UnitY * 25f +Osci * 0.5f;
            context.scale *= 1.1f;
            context.rotation += ToRadians(10);
            context.Draw();
        }
        /// <summary>
        /// 在PostDraw钩子内，位于生命条装饰图层下方
        /// 用于填充生命条与魔力条之间的空缺
        /// </summary>
        /// <param name="context"></param>
        public void DrawNoahRightPanelFill(ResourceOverlayDrawContext context)
        {
            context.texture = ManosabaResource.JougasakiNoahBar.Panel_Mid;
            context.scale *= 1f;
            context.source = context.texture.Frame();
            context.position += Vector2.UnitY * 46f + Vector2.UnitX * 12;
            context.origin = context.texture.Size() / 2;
            context.rotation += ToRadians(0);
            context.Draw();
        }
        /// <summary>
        /// 魔力条装饰，位于postdraw的钩子里
        /// 因为上方调色盘图层问题引起了连锁反应，因此这里的画笔必须得在Predraw直接干掉原有的绘制，然后在postdraw里手动画
        /// </summary>
        /// <param name="context"></param>
        public void DrawNoahPainterMovement(ResourceOverlayDrawContext context)
        {
            NoaDrawTimer += 1f;
            if (ToRadians(360f) < ToRadians(NoaDrawTimer))
                NoaDrawTimer = -360f;
            Osci = Vector2.UnitY * (float)(Math.Sin(ToRadians(NoaDrawTimer)) * 3f);
            context.texture = ManosabaResource.JougasakiNoahBar.Panel_MP_Right;
            context.source = context.texture.Frame();
            context.position += Vector2.UnitX * 15f + Osci - Vector2.UnitY * 5f;
            context.Draw();

        }
    }
    }

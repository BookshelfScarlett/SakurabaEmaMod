using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Globals.Textbox;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Globals.Avator
{
    public static class AvatorMethods
    {
        public static float DrawAvatorWithBackground(DrawableTooltipLine line, IReadOnlyList<TooltipLine> cacheTooltip, ref AvatorSettings avatorSettings, float extraYOffset = 0, float maxWidth = -1)
        {
            if (cacheTooltip is null || line.Index != cacheTooltip.Count - 1)
                return -1;
            DynamicSpriteFont font = line.Font ?? FontAssets.MouseText.Value;
            Vector2 scale = line.BaseScale;
            if (scale == Vector2.Zero)
                scale = Vector2.One;
            if (maxWidth < 0)
            {
                foreach (var t in cacheTooltip)
                {
                    Vector2 size = ChatManager.GetStringSize(font, t.Text, scale);
                    if (size.X > maxWidth)
                        maxWidth = size.X;
                }
            }
            Vector2 avatorSize = avatorSettings.AvatorTexture.Size() * avatorSettings.Scale;
            float spacing = 30f;
            float avatorDrawX = line.X + maxWidth + spacing;
            float avatorDrawY = TextboxManager.FirstLineY + extraYOffset;
            float lerpValue = TextboxManager.LerpValue;
            float edgeValue = TextboxManager.EdgeValue;
            Vector2 posOffset = Vector2.Lerp(Vector2.UnitY * -50f, Vector2.Zero, lerpValue) + Vector2.UnitY * (float)Math.Sin(Main.timeForVisualEffects / 60f) * 10f;
            //边界检查1
            if (avatorDrawX + avatorSize.X > Main.screenWidth)
            {
                avatorDrawX = line.X - avatorSize.X - spacing;
                if (avatorDrawX < 0)
                    avatorDrawX = 0;
            }
            float maxHeight = avatorDrawY + avatorSize.Y + posOffset.Y;
            if (maxHeight > Main.screenHeight)
            {
                //是的这里是面向结果导致的硬编码
                //我目前还没有很好的方案去不使用硬编码，说白了我也不会（
                if (!ManosabaClientConfig.Instance.TraditionalTooltipShowcase)
                {
                    avatorDrawY = TextboxManager.FirstLineY - extraYOffset - 45;
                }
                else
                {
                    float offset = maxHeight - Main.screenHeight;
                    avatorDrawY -= offset;
                    if (avatorDrawY < 0)
                        avatorDrawY = 0;
                }
            }
            Vector2 pos = new Vector2(avatorDrawX, avatorDrawY);
            SpriteBatch sb = Main.spriteBatch;
            DrawAvatorBackground(pos.X, pos.Y, avatorSize.X, avatorSize.Y, 8, pos, avatorSettings.BackgroundColor * lerpValue, posOffset, avatorSettings.BackgroundEdgeColor * edgeValue);
            //开始画头像。
            //注意原点重设为左上角
            sb.Draw(avatorSettings.AvatorTexture, pos + posOffset, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            return maxHeight;
        }
        public static void DrawAvatorBackground(float beginPosX, float beginPosY, float width, float height, int padding, Vector2 drawPos, Color color, Vector2? posOffset = null, Color? edgeColor = null)
        {
            int minX = (int)(beginPosX - padding);
            int minY = (int)(beginPosY - padding);
            int maxX = (int)(beginPosX + width + padding);
            int maxY = (int)(beginPosY + height + padding);
            //设定这个矩形大小
            Rectangle rec = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            Texture2D background = ManosabaTexture.Texture_WhiteCubeBig.Value;
            SpriteBatch sb = Main.spriteBatch;

            Vector2 offset = posOffset ?? Vector2.Zero;
            Rectangle floatingInnerRec = new Rectangle(rec.X + (int)offset.X, rec.Y + (int)offset.Y, rec.Width, rec.Height);

            //内矩形
            int bw = 2;
            Vector2 recPos = drawPos + offset - new Vector2(8);
            //绘制背景，这个背景是一个超级巨大的方块，由于已经超出屏幕，可以直接使用rec的形式随意切割来实现我们需要的效果。
            sb.Draw(background, recPos + new Vector2(bw), floatingInnerRec, color, 0, Vector2.Zero, 1, 0, 0);
            //处理描边
            if (!edgeColor.HasValue)
                return;
            Color borderColor = edgeColor.Value;
            Rectangle outerRec = rec;
            outerRec.Inflate(bw, bw);
            Rectangle floatingOuterRec = new Rectangle(outerRec.X + (int)offset.X, outerRec.Y + (int)offset.Y, outerRec.Width, outerRec.Height);
            Rectangle upRec = new Rectangle(floatingOuterRec.X, floatingOuterRec.Y, floatingOuterRec.Width, bw);
            //上边
            sb.Draw(background, recPos, upRec, borderColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //下边
            sb.Draw(background, recPos + new Vector2(0, floatingOuterRec.Height - bw), upRec, borderColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //左边
            Rectangle leftRec = new Rectangle(floatingOuterRec.X, floatingOuterRec.Y + bw, bw, floatingOuterRec.Height - 2 * bw);
            sb.Draw(background, recPos + new Vector2(0, bw), leftRec, borderColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //这个好像是右边
            sb.Draw(background, recPos + new Vector2(floatingOuterRec.Width - bw, bw), leftRec, borderColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public static void DrawAvatorBackground(Vector2 textPos, Vector2 textSize, Vector2 altTextPos, Vector2 altTextSize, int padding, Vector2 drawPos, Color color, Vector2? posOffset = null, Color? edgeColor = null)
        {
            // 基础矩形（包含内边距）
            int minX = (int)(Math.Min(textPos.X, altTextPos.X) - padding);
            int minY = (int)(Math.Min(textPos.Y, altTextPos.Y) - padding);
            int maxX = (int)(Math.Max(textPos.X + textSize.X, altTextPos.X + altTextSize.X) + padding);
            int maxY = (int)(Math.Max(textPos.Y + textSize.Y, altTextPos.Y + altTextSize.Y) + padding);

            //设定这个矩形大小
            Rectangle rec = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            Texture2D background = ManosabaTexture.Texture_WhiteCubeBig.Value;
            SpriteBatch sb = Main.spriteBatch;
            Vector2 offset = posOffset ?? Vector2.Zero;
            Rectangle floatingInnerRec = new Rectangle(rec.X + (int)offset.X, rec.Y + (int)offset.Y, rec.Width, rec.Height);

            //内矩形
            int bw = 2;
            Vector2 recPos = drawPos + offset - new Vector2(8);
            //绘制背景，这个背景是一个超级巨大的方块，由于已经超出屏幕，可以直接使用rec的形式随意切割来实现我们需要的效果。
            sb.Draw(background, recPos + new Vector2(bw), floatingInnerRec, color, 0, Vector2.Zero, 1, 0, 0);
            if (!edgeColor.HasValue)
                return;
            Color borderColor = edgeColor.Value;
            Rectangle outerRec = rec;
            outerRec.Inflate(bw, bw);
            Rectangle floatingOuterRec = new Rectangle(outerRec.X + (int)offset.X, outerRec.Y + (int)offset.Y, outerRec.Width, outerRec.Height);
            Rectangle upRec = new Rectangle(floatingOuterRec.X, floatingOuterRec.Y, floatingOuterRec.Width, bw);
            //上边
            sb.Draw(background, recPos, upRec, borderColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //下边
            sb.Draw(background, recPos + new Vector2(0, floatingOuterRec.Height - bw), upRec, borderColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //左边
            Rectangle leftRec = new Rectangle(floatingOuterRec.X, floatingOuterRec.Y + bw, bw, floatingOuterRec.Height - 2 * bw);
            sb.Draw(background, recPos + new Vector2(0, bw), leftRec, borderColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //这个好像是右边
            sb.Draw(background, recPos + new Vector2(floatingOuterRec.Width - bw, bw), leftRec, borderColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

    }
}

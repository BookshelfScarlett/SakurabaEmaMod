using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Resources
{
    public partial class ResourceBarOverlayer : ModResourceOverlay
    {
        public static List<RaritySparkle> SakurabaEmaSparkle = [];
        public static List<RaritySparkle> NikaidouHiroSparkle = [];
        public static List<RaritySparkle> NatsumeAnanSparkle = [];
        public static List<RaritySparkle> JougasakniNoaSparkle = [];
        public static List<RaritySparkle> TachibanaSherrySparkle = [];
        private Vector2 Offset => Vector2.UnitX * 45f + Vector2.UnitY;
        private void GeneralParticleManager(ResourceOverlayDrawContext context, bool topLayer = true)
        {
            if (topLayer)
                HandleTopLayerParticle(context);
            else
                HandleLowerLayerParticle(context);
        }
        private void HandleTopLayerParticle(ResourceOverlayDrawContext context)
        {
            bool isTopLayer = CompareAssets(context.texture, "MP_Panel_Right");
            if (!isTopLayer)
                return;

            switch (ManosabaPlayer.ManosabaGirl)
            {
                case ManosabaGirlID.SakurabaEma:
                    DrawEmaParticle(context);
                    break;
                case ManosabaGirlID.NikaidouHiro:
                    DrawHiroParticle(context);
                    break;
                case ManosabaGirlID.NatsumeAnan:
                    DrawAnanParticle(context);
                    break;
                case ManosabaGirlID.JougasakiNoah:
                    DrawNoahPainterMovement(context);
                    break;
                case ManosabaGirlID.TachibanaSherry:
                    DrawSherryParticle(context);
                    break;
                default:
                    break;
            }


        }

        private void DrawSherryParticle(ResourceOverlayDrawContext context)
        {
        }

        private void HandleLowerLayerParticle(ResourceOverlayDrawContext context)
        {
            bool isLowerLayer = CompareAssets(context.texture, "HP_Panel_Right");
            if (!isLowerLayer)
                return;
            if (ManosabaPlayer.ManosabaGirl != ManosabaGirlID.JougasakiNoah)
                return;
            DrawNoahRightPanelFill(context);
            DrawNoaParticle(context);


        }
        private void DrawEmaParticle(ResourceOverlayDrawContext context)
        {
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(22))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f);
                int lifetime = 180;
                //尽量校准位置避免溢出外边框
                //不过yysy溢出了也不会咋样。说实话。
                Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Offset, new Vector2(30)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(Color.LightPink, Color.HotPink).ToAddColor(), lifetime, scale);
                SakurabaEmaSparkle.Add(rarityShinyOrb);
                if (Main.rand.NextBool(3))
                {
                    position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Offset, new Vector2(30)));
                    velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                    SakuraPetals sakuraPetals = new(position, velocity, RandLerpColor(Color.HotPink, Color.LightPink).ToAddColor(100), lifetime, RandRotTwoPi, 1, 0.14f, 0.8f, true);
                    SakurabaEmaSparkle.Add(sakuraPetals);
                }
            }
            UpdateTooltipParticles(Vector2.Zero, SakurabaEmaSparkle);

        }
        private void DrawAnanParticle(ResourceOverlayDrawContext context)
        {

            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(24))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f);
                int lifetime = 180;
                //尽量校准位置避免溢出外边框
                //不过yysy溢出了也不会咋样。说实话。
                Vector2 offset2 = Vector2.UnitX * 30f;
                Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + offset2, new Vector2(30)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(Color.Gold, Color.Yellow).ToAddColor(200), lifetime, scale);
                position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + offset2, new Vector2(30)));
                velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                RarityShinyCrossStar rarityShinyCrossStar = new(position, velocity, RandLerpColor(Color.Gold, Color.Yellow).ToAddColor(50), lifetime, RandRotTwoPi, 1f, scale * 0.5f, 0.2f);
                NatsumeAnanSparkle.Add(rarityShinyOrb);
                NatsumeAnanSparkle.Add(rarityShinyCrossStar);
                if (Main.rand.NextBool(3))
                {
                    position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + offset2, new Vector2(30)));
                    velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                    RarityNoaButterfly rarityNoaButterfly = new RarityNoaButterfly(position, -velocity.SafeNormalize(Vector2.UnitX), RandLerpColor(Color.BlueViolet, Color.SkyBlue).ToAddColor(255), 80, 1, 0.20f, 0.3f, true);
                    NatsumeAnanSparkle.Add(rarityNoaButterfly);
                }
            }
            UpdateTooltipParticles(Vector2.Zero, NatsumeAnanSparkle);

        }

        private void DrawHiroParticle(ResourceOverlayDrawContext context)
        {
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(22))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f);
                int lifetime = 180;
                //尽量校准位置避免溢出外边框
                //不过yysy溢出了也不会咋样。说实话。
                Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Offset, new Vector2(30)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(), lifetime, scale);
                RarityShinyOrb rarityShinyOrb2 = new(position, velocity, Color.White.ToAddColor(), lifetime, scale * 0.5f);
                NikaidouHiroSparkle.Add(rarityShinyOrb);
                NikaidouHiroSparkle.Add(rarityShinyOrb2);
                if (Main.rand.NextBool(3))
                {
                    position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Offset, new Vector2(30)));
                    velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                    SakuraPetals sakuraPetals = new(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(100), lifetime, RandRotTwoPi, 1, 0.14f, 0.8f, true);
                    NikaidouHiroSparkle.Add(sakuraPetals);
                }
            }
            UpdateTooltipParticles(Vector2.Zero, NikaidouHiroSparkle);
        }
        private void DrawNoaParticle(ResourceOverlayDrawContext context)
        {
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(20))
            {
                float scale = 0.1f;
                int lifetime = 180;
                //尽量校准位置避免溢出外边框
                //不过yysy溢出了也不会咋样。说实话。
                Vector2 offset2 = Vector2.UnitX * 20f - Vector2.UnitY * 15f;
                Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Offset - offset2, new Vector2(30)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.5f, 5.55f);
                Color[] randColor = [Color.Red, Color.SkyBlue, Color.Yellow, Color.Green, Color.White];
                Color randColor2 = Utils.SelectRandom(Main.rand, randColor);
                RarityFullCircle rarityFullCircle = new(position, velocity, randColor2, lifetime, scale);
                RarityFullCircle rarityFullCircle2 = new(position + velocity.SafeNormalize(Vector2.UnitX) * 10f, velocity, randColor2, lifetime, scale);
                JougasakniNoaSparkle.Add(rarityFullCircle2);
                //JougasakniNoaSparkle.Add(rarityShinyOrb2);
            }
            UpdateTooltipParticles(Vector2.Zero, JougasakniNoaSparkle);
        }

        /// <summary>
        /// 手动操作稀有度粒子的更新
        /// 需注意的是这里的sparkle实际上只有一个列表并且继承raritysparkle
        /// 但主要是我也不想管理更多东西了。
        /// </summary>
        /// <param name="sparklesList"></param>
        public static void UpdateTooltipParticles(Vector2 drawPos, List<RaritySparkle> sparklesList)
        {
            //在这里更新粒子的Draw，让粒子动起来
            for (int i = 0; i < sparklesList.Count; i++)
            {
                sparklesList[i].CustomUpdate();
                sparklesList[i].Time++;
            }
            //在需要的时候删除掉粒子，即使用的粒子系统内的LifeTimeRatio
            sparklesList.RemoveAll((s) => s.LifetimeRatio >= 1);
            //而后，绘制所有的粒子
            foreach (RaritySparkle sparkle in sparklesList)
                sparkle.CustomDraw(Main.spriteBatch, drawPos + sparkle.Position);
        }
    }
}

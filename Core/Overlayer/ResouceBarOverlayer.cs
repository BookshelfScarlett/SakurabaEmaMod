using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Globals.Players;
using SakurabaEmaMod.Items.Vanity;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core.Overlayer
{
    /// <summary>
    /// 这里的码从tmod案例抄写过来的
    /// </summary>
    public class ResouceBarOverlayer : ModResourceOverlay
    {
        // This field is used to cache vanilla assets used in the CompareAssets helper method further down in this file
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();

        // These fields are used to cache the result of ModContent.Request<Texture2D>()
        private Asset<Texture2D> heartTexture, fancyPanelTexture, barsFillingTexture, barsPanelTexture;
        //shorthand
        public ManosabaPlayer ManosabaPlayer => Main.LocalPlayer.ManosabaMod();
        public bool DrawCharactorResoure(ManosabaResourceSet setter, ResourceOverlayDrawContext context)
        {
            if (CompareAllAssets(setter, context, true))
                return false;
            else if (CompareAllAssets(setter, context, false))
                return false;
            else
                return true;
        }
        public bool CompareAllAssets(ManosabaResourceSet setter, ResourceOverlayDrawContext context, bool hp)
        {
            string prefix = hp ? "HP" : "MP";
            if (hp)
            {
                if (CompareAssets(context.texture, "HP_Panel_Middle"))
                {
                    context.texture = setter.Panel_HP_Mid;
                    context.source = context.texture.Frame();
                    context.Draw();
                    return true;
                }
                if (CompareAssets(context.texture, "HP_Fill") || CompareAssets(context.texture, "HP_Fill_Honey"))
                {
                    context.texture = setter.Panel_HP_Fill;
                    context.source = context.texture.Frame();
                    context.Draw();
                    return true;
                }
                if (CompareAssets(context.texture, "HP_Panel_Right"))
                {
                    context.texture = setter.Panel_HP_Right;
                    context.source = context.texture.Frame();
                    context.position.X -= 2;
                    context.Draw();
                    return true;
                }
            }
            else
            {
                if (CompareAssets(context.texture, "MP_Panel_Middle"))
                {
                    context.texture = setter.Panel_MP_Mid;
                    context.source = context.texture.Frame();
                    context.Draw();
                    return true;
                }
                if (CompareAssets(context.texture, "MP_Panel_Right"))
                {
                    context.texture = setter.Panel_MP_Right;
                    context.source = context.texture.Frame();
                    context.position.X -= 3;
                    context.Draw();
                    return true;
                }
                if (CompareAssets(context.texture, "MP_Fill"))
                {
                    context.texture = setter.Panel_HP_Fill_Honey;
                    context.source = context.texture.Frame();
                    context.Draw();
                    return true;
                }

            }
            if (CompareAssets(context.texture, "Panel_Left"))
            {
                context.texture = setter.Panel_Left;
                context.source = context.texture.Frame();
                context.Draw();
                return true;
            }
            return false;
        }
        public override bool PreDrawResource(ResourceOverlayDrawContext context)
        {
            if (Main.LocalPlayer.GetModPlayer<SakurabaEmaPlayer>().vanityEquipped)
            {
                if (CompareAllAssets(ManosabaResource.SakurabaEmaBar, context, true))
                {
                    return false;

                }
                else if (CompareAllAssets(ManosabaResource.SakurabaEmaBar, context, false))
                {
                    return false;
                }
            }
            //奖池还在积累
            //这地方必须得重写
            switch (ManosabaPlayer.ManosabaGirl)
            {
                case ManosabaGirlID.NikaidouHiro:
                    if (CompareAllAssets(ManosabaResource.NikaidouHiroBar, context, true))
                        return false;
                    else if (CompareAllAssets(ManosabaResource.NikaidouHiroBar, context, false))
                        return false;
                    return true;
                default:
                    return true;
            }
        }
        public static List<RaritySparkle> SakurabaEmaSparkle = [];
        public static List<RaritySparkle> NikaidouHiroSparkle = [];
        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            if (Main.LocalPlayer.GetModPlayer<SakurabaEmaPlayer>().vanityEquipped)
            {
                if (CompareAssets(context.texture, "HP_Panel_Right"))
                {
                    //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
                    //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
                    if (Main.rand.NextBool(22))
                    {
                        float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f);
                        int lifetime = 180;
                        //尽量校准位置避免溢出外边框
                        //不过yysy溢出了也不会咋样。说实话。
                        Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Vector2.UnitX * 33f + Vector2.UnitY * 20f, new Vector2(30)));
                        Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                        RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(Color.LightPink, Color.HotPink).ToAddColor(), lifetime, scale);
                        SakurabaEmaSparkle.Add(rarityShinyOrb);
                        if (Main.rand.NextBool(3))
                        {
                            position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Vector2.UnitX * 33f + Vector2.UnitY * 20f, new Vector2(30)));
                            velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                            SakuraPetals sakuraPetals = new(position, velocity, RandLerpColor(Color.HotPink, Color.LightPink).ToAddColor(100), lifetime, RandRotTwoPi, 1, 0.14f, 0.8f, true);
                            SakurabaEmaSparkle.Add(sakuraPetals);
                        }
                    }
                    UpdateTooltipParticles(Vector2.Zero, ref SakurabaEmaSparkle);
                }
            }
            switch (ManosabaPlayer.ManosabaGirl)
            {
                case ManosabaGirlID.NikaidouHiro:
                    DrawHiroParticle(context);
                    break;
                default:
                    break;
            }
        }

        private void DrawHiroParticle(ResourceOverlayDrawContext context)
        {
            if (!CompareAssets(context.texture, "HP_Panel_Right"))
                return;
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            //因为没有实际使用一个总的粒子列表来控制所有的粒子绘制，因此这里都是要进行手动操作的
            if (Main.rand.NextBool(22))
            {
                float scale = Main.rand.NextFloat(0.40f * 0.5f, 0.40f);
                int lifetime = 180;
                //尽量校准位置避免溢出外边框
                //不过yysy溢出了也不会咋样。说实话。
                Vector2 position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Vector2.UnitX * 33f + Vector2.UnitY * 20f, new Vector2(30)));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(), lifetime, scale);
                RarityShinyOrb rarityShinyOrb2 = new(position, velocity, Color.White.ToAddColor(), lifetime, scale * 0.5f);
                NikaidouHiroSparkle.Add(rarityShinyOrb);
                NikaidouHiroSparkle.Add(rarityShinyOrb2);
                if (Main.rand.NextBool(3))
                {
                    position = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(context.position + Vector2.UnitX * 33f + Vector2.UnitY * 20f, new Vector2(30)));
                    velocity = Vector2.UnitY * Main.rand.NextFloat(0.05f, 3.55f);
                    SakuraPetals sakuraPetals = new(position, velocity, RandLerpColor(Color.Crimson, Color.DarkRed).ToAddColor(100), lifetime, RandRotTwoPi, 1, 0.14f, 0.8f, true);
                    NikaidouHiroSparkle.Add(sakuraPetals);
                }
            }
            UpdateTooltipParticles(Vector2.Zero, ref NikaidouHiroSparkle);
        }


        /// <summary>
        /// 手动操作稀有度粒子的更新
        /// 需注意的是这里的sparkle实际上只有一个列表并且继承raritysparkle
        /// 但主要是我也不想管理更多东西了。
        /// </summary>
        /// <param name="sparklesList"></param>
        public static void UpdateTooltipParticles(Vector2 drawPos, ref List<RaritySparkle> sparklesList)
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

        private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath)
        {
            string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";
            // This is a helper method for checking if a certain vanilla asset was drawn
            if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
                asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(barsFolder + compareAssetPath);

            return existingAsset == asset;
        }
    }
}

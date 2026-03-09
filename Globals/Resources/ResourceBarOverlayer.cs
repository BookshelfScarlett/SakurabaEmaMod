using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Globals.Players;
using SakurabaEmaMod.Items.Vanity;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using SakurabaEmaMod.Rarity.RarityParticles;
using SakurabaEmaMod.Rarity.RarityShiny;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Resources
{
    /// <summary>
    /// 这里的码从tmod案例抄写过来的
    /// </summary>
    public partial class ResourceBarOverlayer : ModResourceOverlay
    {
        // This field is used to cache vanilla assets used in the CompareAssets helper method further down in this file
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = [];
        //shorthand
        public ManosabaPlayer ManosabaPlayer => Main.LocalPlayer.ManosabaMod();
        public override bool PreDrawResource(ResourceOverlayDrawContext context)
        {
            if (!ManosabaClientConfig.Instance.UseCharactorLifeBar)
                return true;
            if (Main.LocalPlayer.GetModPlayer<SakurabaEmaPlayer>().vanityEquipped)
            {
                if (CompareAllAssets(ManosabaResource.SakurabaEmaBar, context))
                {
                    return false;
                }
            }
            //奖池还在积累
            //这地方必须得重写
            Dictionary<short, ManosabaResourceSet> _RarityMap = new()
            {
                { ManosabaGirlID.NatsumeAnan, ManosabaResource.NatsumeAnanBar},
                { ManosabaGirlID.NikaidouHiro, ManosabaResource.NikaidouHiroBar},
                { ManosabaGirlID.JougasakiNoah, ManosabaResource.JougasakiNoahBar}
            };
            if (_RarityMap.TryGetValue(ManosabaPlayer.ManosabaGirl, out var value))
                return !CompareAllAssets(value, context);
            return true;
        }


        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            //绘制粒子前的准备工作
            bool isTopLayer = CompareAssets(context.texture, "MP_Panel_Right");
            if (!ManosabaClientConfig.Instance.UseCharactorLifeBar)
                return;
            GeneralParticleManager(context, isTopLayer);
        }
        public bool CompareAllAssets(ManosabaResourceSet setter, ResourceOverlayDrawContext context)
        {
            if (CompareAssets(context.texture, "HP_Panel_Right"))
            {
                //诺亚的必须得给个特判，不要在这里画而是在魔力条那画
                if (setter.Equals(ManosabaResource.JougasakiNoahBar))
                {
                    return true;
                }
                context.texture = setter.Panel_HP_Right;
                context.source = context.texture.Frame();
                context.position.X -= 1;
                context.Draw();
                return true;
            }
            if (CompareAssets(context.texture, "HP_Panel_Middle") || CompareAssets(context.texture, "MP_Panel_Middle"))
            {
                context.texture = setter.Panel_Mid;
                context.source = context.texture.Frame();
                context.Draw();
                return true;
            }
            if (CompareAssets(context.texture, "HP_Fill"))
            {
                context.texture = setter.Panel_HP_Fill;
                context.source = context.texture.Frame();
                context.Draw();
                return true;
            }
            if (CompareAssets(context.texture, "HP_Fill_Honey"))
            {
                context.texture = setter.Panel_HP_Fill;
                context.Draw();
                return true;
            }

            if (CompareAssets(context.texture, "MP_Panel_Right"))
            {
                //诺亚的必须得给个特判
                if (setter.Equals(ManosabaResource.JougasakiNoahBar))
                {
                    DrawNoahLifeRightPanel(context);
                    return true;
                }
                context.texture = setter.Panel_MP_Right;
                context.source = context.texture.Frame();
                context.position.X -= 2;
                context.Draw();
                return true;
            }
            if (CompareAssets(context.texture, "MP_Fill"))
            {
                context.texture = setter.Panel_MP_Fill;
                context.Draw();
                return true;
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
        public bool DrawCharactorResoure(ManosabaResourceSet setter, ResourceOverlayDrawContext context)
        {
            if (CompareAllAssets(setter, context))
                return false;
            else
                return true;
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

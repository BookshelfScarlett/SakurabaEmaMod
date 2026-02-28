using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Rarity.RarityShiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core
{
    public class ManosabaRaritySystem : ModSystem
    {
        public  static ManosabaRaritySystem Instance;
        private Dictionary<short, int> _RarityMap;
        private Dictionary<short , CharactorRarity> _EffectMap;
        public override void Load()
        {
            Instance = this;
            _RarityMap = new Dictionary<short, int>()
            {
                { ManosabaGirlID.SakurabaEma, RarityType<SakurabaEmaRarity>()},
                { ManosabaGirlID.NatsumeAnan, RarityType<NatsumeAnanRarity>()},
                { ManosabaGirlID.JougasakiNoa , RarityType<JougasakiNoaRarity>()},
            };
            _EffectMap = new Dictionary<short , CharactorRarity>()
            {
                {ManosabaGirlID.SakurabaEma, new NatsumeAnanRarity()},
                {ManosabaGirlID.NatsumeAnan , new NatsumeAnanRarity()},
                {ManosabaGirlID.JougasakiNoa, new JougasakiNoaRarity()}
            };
        }
        public bool DrawRarityEffect(DrawableTooltipLine line, short charactor)
        {
            var effect = GetCurrentRarityEffect(charactor);
            if (effect is null)
                return true;

            if (line.IsItemName())
            {
                effect.DrawItemName(line);
                return false;
            }
            if (line.IsThisLine("SinnerTypeName", Mod.Name))
            {
                effect.DrawFlavorName(line);
                return false;
            }
            if (line.IsThisLine("SinnerTooltipName", Mod.Name))
            {
                effect.DrawFlavorTooltip(line);
                return false;
            }
            if (line.IsThisLine("Vanity") || line.IsThisLine("Equipable"))
            {
                effect.DrawMisc(line);
                return false;
            }
            return true;
        }
        public int SetRarityType(short key)
        {
            if (_RarityMap.TryGetValue(key, out var value))
            {
                return value;
            }
            return ItemRarityID.Green;
        }
        private CharactorRarity GetCurrentRarityEffect(short key)
        { 
            if (_EffectMap.TryGetValue(key, out var effect))
            {
                return effect;
            }
            return null;
        }
    }
    public class ManosabaModSystem : ModSystem
    {
        public static Vector2 ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        public static Rectangle MouseRectangle;
        public override void UpdateUI(GameTime gameTime)
        {
            ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            MouseRectangle = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 4, 4);
        }
    }
}

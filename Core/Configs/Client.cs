using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace SakurabaEmaMod.Core.Configs
{
    public class ManosabaClientConfig : ModConfig
    {
        public static ManosabaClientConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => false;
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool ParticleDontEmitLight { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool NoFinalBloomPlay { get; set; }
    }
}

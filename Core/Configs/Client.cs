using System.ComponentModel;
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
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool UseCharactorLifeBar { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool TraditionalTooltipShowcase{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool NoCustomCharactorSounds { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool NoCharactorAvatar { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool NoCharactorParticle { get; set; }


    }
}

using SakurabaEmaMod.Globals.Enums;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaSounds : ModSystem
    {

        private static string Path => "SakurabaEmaMod/Assets/Sounds/";
        public static SoundStyle Ema_HitSound;
        public static SoundStyle Ema_HitHeavy ;
        public static SoundStyle Ema_Kiang;
        public static SoundStyle GetSound(Charactor charactor, string soundName, int soundVariants = 1)
        {
            return new SoundStyle($"{Path}{charactor}Sounds/{soundName}", numVariants: soundVariants);
        }
        public override void Load()
        {
            Ema_HitSound = GetSound(Charactor.SakurabaEma, nameof(Ema_HitSound), 4);
            Ema_HitHeavy = GetSound(Charactor.SakurabaEma, nameof(Ema_HitHeavy), 3);
            Ema_Kiang = GetSound(Charactor.SakurabaEma, nameof(Ema_Kiang));
        }
    }
}

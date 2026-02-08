using Terraria.Audio;

namespace SakurabaEmaMod.Assets.Register
{
    public class SoundRegister
    {
        public enum Charactor
        {
            SakurabaEma,
            NikaidouHiro,
            NatsumeAnn,
            JougasakiNoa,
            TachibanaSherry,
            ToonoHanna
        }
        private static string Path => "SakurabaEmaMod/Assets/Sounds/";
        public static SoundStyle Ema_HitSound = GetSound(Charactor.SakurabaEma, nameof(Ema_HitSound), 4);
        public static SoundStyle Ema_HitHeavy = GetSound(Charactor.SakurabaEma, nameof(Ema_HitHeavy), 3);
        public static SoundStyle Ema_Kiang = GetSound(Charactor.SakurabaEma, nameof(Ema_Kiang));
        public static SoundStyle GetSound(Charactor charactor, string soundName, int soundVariants = 1)
        {
            return new SoundStyle($"{Path}{charactor}Sounds/{soundName}", numVariants: soundVariants);
        }
    }
}

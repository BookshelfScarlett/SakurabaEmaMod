using SakurabaEmaMod.Globals.Enums;
using Steamworks;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaSounds : ModSystem
    {
        private static string Path => "SakurabaEmaMod/Assets/Sounds/";
        public static SoundStyle Ema_HitSound;
        public static SoundStyle Ema_HitHeavy;
        public static SoundStyle Ema_Kiang;

        public static SoundStyle Anan_Hit;
        public static SoundStyle Anan_Death;
        public static SoundStyle Hiro_Hit;
        public static SoundStyle Hiro_Death;
        public static SoundStyle Noah_Hit;
        public static SoundStyle Noah_Cry;
        public static SoundStyle Noah_SpecialCry;
        public static SoundStyle Noah_SpecialDeath;

        public static SoundStyle Menu_Cancel => new SoundStyle($"{Path}System/{nameof(Menu_Cancel)}");
        public static SoundStyle Menu_GeneralChoice => new SoundStyle($"{Path}System/{nameof(Menu_GeneralChoice)}");
        public static SoundStyle Menu_LoadGame => new SoundStyle($"{Path}System/{nameof(Menu_LoadGame)}");
        public static SoundStyle Menu_MainChoice => new SoundStyle($"{Path}System/{nameof(Menu_MainChoice)}");
        public static SoundStyle Menu_Notiflication => new SoundStyle($"{Path}System/{nameof(Menu_Notiflication)}");
        public static SoundStyle Menu_Switch => new SoundStyle($"{Path}System/{nameof(Menu_Switch)}");
        public static SoundStyle GetSound(Charactor charactor, string soundName, int soundVariants = 1)
        {
            return new SoundStyle($"{Path}{charactor}Sounds/{soundName}", numVariants: soundVariants);
        }
        public override void Load()
        {
            Ema_HitSound = GetSound(Charactor.SakurabaEma, nameof(Ema_HitSound), 4);
            Ema_HitHeavy = GetSound(Charactor.SakurabaEma, nameof(Ema_HitHeavy), 3);
            Ema_Kiang = GetSound(Charactor.SakurabaEma, nameof(Ema_Kiang));

            Anan_Hit = GetSound(Charactor.NatsumeAnan, "Hit", 5);
            Anan_Death = GetSound(Charactor.NatsumeAnan, "Death", 2);

            Hiro_Hit = GetSound(Charactor.NikaidouHiro, "Hit", 4);
            Hiro_Death = GetSound(Charactor.NikaidouHiro, "Death", 2);

            Noah_Hit = GetSound(Charactor.JougasakiNoah, "Hit", 6);
            Noah_Cry = GetSound(Charactor.JougasakiNoah, "Cry", 6);
            Noah_SpecialCry = GetSound(Charactor.JougasakiNoah, "Special");
            Noah_SpecialDeath = GetSound(Charactor.JougasakiNoah, "SpecialDeath");
        }
    }
}

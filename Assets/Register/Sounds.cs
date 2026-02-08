using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;

namespace SakurabaEmaMod.Assets.Register
{
    public class SoundRegister
    {
        public enum Charactor
        {
            SakurabaEma
        }
        private static string Path => "SakurabaEmaMod/Assets/Sounds/";
        public static SoundStyle Ema_HitSound = new SoundStyle($"{Path}{Charactor.SakurabaEma}Sounds/{nameof(Ema_HitSound)}", numVariants: 4);
        public static SoundStyle Ema_Kiang = new SoundStyle($"{Path}{Charactor.SakurabaEma}Sounds/{nameof(Ema_Kiang)}");
        public static SoundStyle Ema_HitHeavy= new SoundStyle($"{Path}{Charactor.SakurabaEma}Sounds/{nameof(Ema_HitHeavy)}", 3);
    }
}

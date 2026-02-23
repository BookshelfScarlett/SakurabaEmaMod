using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaMusic : ModSystem
    {
        private static string Path => "SakurabaEmaMod/Assets/Music/";
        public static string Menu = $"{Path}{nameof(Menu)}";

    }
}

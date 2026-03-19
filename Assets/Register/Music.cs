using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaMusic : ModSystem
    {
        private static string Path => "SakurabaEmaMod/Assets/Music/";
        public static string Menu = $"{Path}{nameof(Menu)}";
        public static string None = $"{Path}{nameof(None)}";

    }
}

using Microsoft.Xna.Framework.Media;
using ReLogic.Content;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaVideo : ModSystem
    {
        public static Asset<Video> BloomPV { get; private set; }
        public override void Load()
        {
            BloomPV = Request<Video>("SakurabaEmaMod/Assets/Videos/Bloom");
        }
        public override void Unload()
        {
            BloomPV = null;
        }
    }
}

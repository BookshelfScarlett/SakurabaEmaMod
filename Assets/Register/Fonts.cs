using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaFonts : ModSystem
    {
        private string Path = "SakurabaEmaMod/Assets/Texture/Fonts/";
        public static Asset<DynamicSpriteFont> 等线 { get; private set; }
        public static Asset<DynamicSpriteFont> BookAntiqua { get; private set; }
        public override void Load()
        {
            等线 = Request<DynamicSpriteFont>($"{Path}等线");
            BookAntiqua = Request<DynamicSpriteFont>($"{Path}BookAntiqua");
        }
        public override void Unload()
        {
            等线 = null;
            BookAntiqua = null;
        }
    }
}

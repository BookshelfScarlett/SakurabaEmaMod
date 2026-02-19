using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus
{
    public class MenuTexPath
    {
        public Asset<Texture2D> Texture { get; }
        public string Path { get; }
        public MenuTexPath(Asset<Texture2D> texture, string path)
        {
            Path = path;
            Texture = texture;
        }
        public MenuTexPath(string path)
        {
            Path = path;
            Texture = Request<Texture2D>($"{Path}");
        }
    }
    public class ManosabaMenuAssets : ModSystem
    {
        private string Path => "SakurabaEmaMod/Assets/Texture/Menu/";
        public static MenuTexPath Main_ExitChosen { get; private set; }
        public static MenuTexPath Main_ExitUnChosen { get; private set; }
        public static MenuTexPath Main_GalleryChosen { get; private set; }
        public static MenuTexPath Main_GalleryUnChosen { get; private set; }
        public static MenuTexPath Main_LoadGameChosen {  get; private set; }
        public static MenuTexPath Main_LoadGameUnChosen {  get; private set; }
        public static MenuTexPath Main_OptionChosen {  get; private set; }
        public static MenuTexPath Main_OptionUnChosen {  get; private set; }
        public static MenuTexPath Main_EmaBackground{  get; private set; }
        public static MenuTexPath Main_Mask {  get; private set; }
        public static MenuTexPath Main_Title {  get; private set; }
        public static MenuTexPath Alt_ButtonChosen {  get; private set; }
        public static MenuTexPath Alt_ButtonUnChosen {  get; private set; }
        public static MenuTexPath Alt_ExitUnChosen { get; private set; }
        public static MenuTexPath Alt_CornerDeco {  get; private set; }
        public static MenuTexPath Alt_Mask {  get; private set; }
        public override void Load()
        {
            Main_ExitChosen = new MenuTexPath($"{Path}{nameof(Main_ExitChosen)}");
            Main_ExitUnChosen = new MenuTexPath($"{Path}{nameof(Main_ExitUnChosen)}");
            Main_OptionUnChosen = new MenuTexPath($"{Path}{nameof(Main_OptionUnChosen)}");
            Main_OptionChosen = new MenuTexPath($"{Path}{nameof(Main_OptionChosen)}");
            Main_GalleryChosen = new MenuTexPath($"{Path}{nameof(Main_GalleryChosen)}");
            Main_GalleryUnChosen = new MenuTexPath($"{Path}{nameof(Main_GalleryUnChosen)}");
            Main_LoadGameChosen = new MenuTexPath($"{Path}{nameof(Main_LoadGameChosen)}");
            Main_LoadGameUnChosen = new MenuTexPath($"{Path}{nameof(Main_LoadGameUnChosen)}");
            Main_EmaBackground = new MenuTexPath($"{Path}{nameof(Main_EmaBackground)}");
            Main_Mask = new MenuTexPath($"{Path}{nameof(Main_Mask)}");
            Main_Title = new MenuTexPath($"{Path}{nameof(Main_Title)}");

            Alt_ButtonChosen = new MenuTexPath($"{Path}{nameof(Alt_ButtonChosen)}");
            Alt_ButtonUnChosen = new MenuTexPath($"{Path}{nameof(Alt_ButtonUnChosen)}");
            Alt_ExitUnChosen = new MenuTexPath($"{Path}{nameof(Alt_ExitUnChosen)}");
            Alt_CornerDeco = new MenuTexPath($"{Path}{nameof(Alt_CornerDeco)}");
        }
        public override void Unload()
        {
            Main_ExitChosen = null;
            Main_ExitUnChosen = null;
            Main_GalleryChosen = null;
            Main_GalleryUnChosen = null;
            Main_OptionChosen = null;
            Main_OptionUnChosen = null;
            Main_LoadGameChosen = null;
            Main_LoadGameUnChosen = null;
            Main_EmaBackground = null;
            Main_Mask = null;
            Main_Title = null;

            Alt_ButtonChosen = null;
            Alt_ButtonUnChosen = null;
            Alt_ExitUnChosen = null;
            Alt_CornerDeco = null;
            Alt_Mask = null;
        }
    }
}

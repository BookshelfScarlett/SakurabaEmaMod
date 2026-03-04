using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
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
        private string StillPath => "SakurabaEmaMod/Assets/Texture/Stills/";
        public static MenuTexPath Main_ExitChosen { get; private set; }
        public static MenuTexPath Main_ExitUnChosen { get; private set; }
        public static MenuTexPath Main_GalleryChosen { get; private set; }
        public static MenuTexPath Main_GalleryUnChosen { get; private set; }
        public static MenuTexPath Main_LoadGameChosen {  get; private set; }
        public static MenuTexPath Main_LoadGameUnChosen {  get; private set; }
        public static MenuTexPath Main_OptionChosen {  get; private set; }
        public static MenuTexPath Main_OptionUnChosen {  get; private set; }
        public static MenuTexPath Still_Ema{  get; private set; }
        public static MenuTexPath Still_Hiro{  get; private set; }
        public static MenuTexPath Still_Anan{  get; private set; }
        public static MenuTexPath Still_Noa{  get; private set; }
        public static MenuTexPath Still_HannaSherry{  get; private set; }
        public static MenuTexPath Still_YukiMeruru{  get; private set; }
        public static MenuTexPath Still_Margo{  get; private set; }
        public static MenuTexPath Main_Mask {  get; private set; }
        public static MenuTexPath Main_Title {  get; private set; }
        public static MenuTexPath Main_ArrowLeft { get; private set; }
        public static MenuTexPath Main_ArrowRight { get; private set; }
        public static MenuTexPath Alt_ButtonChosen {  get; private set; }
        public static MenuTexPath Alt_ButtonUnChosen {  get; private set; }
        public static MenuTexPath Alt_ExitUnChosen { get; private set; }
        public static MenuTexPath Alt_ExitChosen { get; private set; }
        public static MenuTexPath Alt_CornerDeco {  get; private set; }
        public static MenuTexPath Alt_Mask {  get; private set; }
        public static MenuTexPath[] ManosabaBackgroundList = new MenuTexPath[5];
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
            Still_Ema = new MenuTexPath($"{StillPath}{nameof(Still_Ema)}");
            Still_Hiro = new MenuTexPath($"{StillPath}{nameof(Still_Hiro)}");
            Still_Anan = new MenuTexPath($"{StillPath}{nameof(Still_Anan)}");
            Still_Noa  = new MenuTexPath($"{StillPath}{nameof(Still_Noa)}");
            Still_HannaSherry = new MenuTexPath($"{StillPath}{nameof(Still_HannaSherry)}");
            Still_YukiMeruru = new MenuTexPath($"{StillPath}{nameof(Still_YukiMeruru)}");
            Still_Margo = new MenuTexPath($"{StillPath}{nameof(Still_Margo)}");
            Main_Mask = new MenuTexPath($"{Path}{nameof(Main_Mask)}");
            Main_Title = new MenuTexPath($"{Path}{nameof(Main_Title)}");
            Main_ArrowLeft = new MenuTexPath($"{Path}{nameof(Main_ArrowLeft)}");
            Main_ArrowRight = new MenuTexPath($"{Path}{nameof(Main_ArrowRight)}");

            Alt_ButtonChosen = new MenuTexPath($"{Path}{nameof(Alt_ButtonChosen)}");
            Alt_ButtonUnChosen = new MenuTexPath($"{Path}{nameof(Alt_ButtonUnChosen)}");
            Alt_ExitUnChosen = new MenuTexPath($"{Path}{nameof(Alt_ExitUnChosen)}");
            Alt_ExitChosen = new MenuTexPath($"{Path}{nameof(Alt_ExitChosen)}");
            Alt_CornerDeco = new MenuTexPath($"{Path}{nameof(Alt_CornerDeco)}");
            Alt_Mask = new MenuTexPath($"{Path}{nameof(Alt_Mask)}");

            //完成内容的实例化后再把这些东西扔进数组内调用
            ManosabaBackgroundList = [Still_Ema, Still_Hiro, Still_Anan, Still_Noa, Still_HannaSherry, Still_YukiMeruru, Still_Margo];
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

            Main_Mask = null;
            Main_Title = null;
            Main_ArrowLeft = null;
            Main_ArrowRight = null;
   
            Still_Ema = null;
            Still_Hiro = null;
            Still_Anan = null;
            Still_HannaSherry = null;
            Still_Noa = null;
            Still_YukiMeruru = null;
            Still_Margo = null;

            Alt_ButtonChosen = null;
            Alt_ButtonUnChosen = null;
            Alt_ExitUnChosen = null;
            Alt_CornerDeco = null;
            Alt_Mask = null;
            Alt_ExitChosen = null;
            ManosabaBackgroundList = null;
        }
    }
}

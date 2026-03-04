using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SakurabaEmaMod.Globals.Enums;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public sealed class ManosabaResourceSet
    {
        public Asset<Texture2D> Panel_HP_Right { get; }
        public Asset<Texture2D> Panel_HP_Mid { get; }
        public Asset<Texture2D> Panel_HP_Fill { get; }
        public Asset<Texture2D> Panel_HP_Fill_Honey { get; }
        public Asset<Texture2D> Panel_Left { get; }
        public Asset<Texture2D> Panel_MP_Right { get; }
        public Asset<Texture2D> Panel_MP_Mid { get; }
        public Asset<Texture2D> Panel_MP_Fill { get; }
        public ManosabaResourceSet(Charactor charactor)
        {
            string SourcePath = $"SakurabaEmaMod/Assets/Texture/CharactorSets/{charactor}/";
            Panel_HP_Right = Request<Texture2D>(SourcePath + nameof(Panel_HP_Right));
            Panel_HP_Mid = Request<Texture2D>(SourcePath + nameof(Panel_HP_Mid));
            Panel_HP_Fill = Request<Texture2D>(SourcePath + nameof(Panel_HP_Fill));
            Panel_HP_Fill_Honey = Request<Texture2D>(SourcePath + nameof(Panel_HP_Fill_Honey));
            Panel_Left = Request<Texture2D>(SourcePath + nameof(Panel_Left));
            Panel_MP_Right = Request<Texture2D>(SourcePath + nameof(Panel_MP_Right));
            Panel_MP_Mid = Request<Texture2D>(SourcePath + nameof(Panel_MP_Mid));
            Panel_MP_Fill = Request<Texture2D>(SourcePath + nameof(Panel_MP_Fill));
        }
    }
    public class ManosabaResource : ModSystem
    {
        public static ManosabaResourceSet SakurabaEmaBar;
        public static ManosabaResourceSet NikaidouHiroBar;
        public static ManosabaResourceSet NatsumeAnanBar;
        public override void Load()
        {
            SakurabaEmaBar = new ManosabaResourceSet(Charactor.SakurabaEma);
            NikaidouHiroBar = new ManosabaResourceSet(Charactor.NikaidouHiro);
            NatsumeAnanBar = new ManosabaResourceSet(Charactor.NatsumeAnan);
        }
        public override void Unload()
        {
            SakurabaEmaBar = null;
            NikaidouHiroBar = null;
            NatsumeAnanBar = null;
        }
    }
}

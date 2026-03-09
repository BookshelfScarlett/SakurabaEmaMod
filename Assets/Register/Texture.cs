using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    // todo：这里的注册系统完全没有必要……
    public class Tex2DWithPath
    {
        public Asset<Texture2D> Texture { get; }
        public string Path { get; }
        public Tex2DWithPath(Asset<Texture2D> texture, string path)
        {
            Path = path;
            Texture = texture;
        }
        public Tex2DWithPath(string path)
        {
            Path = path;
            Texture = Request<Texture2D>($"{Path}");
        }
        public Texture2D Value => Texture.Value;
        public int Height => Texture.Height();
        public int Width => Texture.Width();
        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
        }
        public Vector2 Origin
        {
            get
            {
                return Size / 2;
            }
        }
    }
    public class ManosabaTexture : ModSystem
    {
        public static Tex2DWithPath Particle_ShinyOrb { get; set; }
        public static Tex2DWithPath Particle_HRShinyOrbSmall { get; set; }
        public static Tex2DWithPath Particle_FusableBall { get; set; }
        public static Tex2DWithPath Particle_Leafs { get; set; }
        public static Tex2DWithPath Particle_CrossGlow { get; set; }
        public static Tex2DWithPath Particle_Petal { get; set; }
        public static Tex2DWithPath Particle_NoaButterfly { get; set; }
        public static Tex2DWithPath Particle_FullCircle{ get; set; }
        public static Tex2DWithPath Particle_FullCircleHard{ get; set; }

        public static Tex2DWithPath Texture_BloomShockwave { get; set; }
        public static Tex2DWithPath Texture_RarityGlow { get; set; }

        public static Tex2DWithPath InvisAsset { get; private set; }
        public static Texture2D Particle_SharpTear => TextureAssets.Extra[ExtrasID.SharpTears].Value;
        private string TexPath => "SakurabaEmaMod/Assets/Texture";
        private string Path_Particle => $"{TexPath}/Particles/";
        private string Path_General => $"{TexPath}/General/";
        private static string InvisAssetPath => "SakurabaEmaMod/Assets/Texture/InvisibleProj";
        public override void Load()
        {
            InvisAsset = new Tex2DWithPath(InvisAssetPath);

            Particle_ShinyOrb = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_ShinyOrb)}");
            Particle_HRShinyOrbSmall = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_HRShinyOrbSmall)}");
            Particle_Leafs = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_Leafs)}");
            Particle_CrossGlow = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_CrossGlow)}");
            Particle_Petal = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_Petal)}");
            Particle_NoaButterfly = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_NoaButterfly)}");
            Particle_FusableBall = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_FusableBall)}");
            Particle_FullCircle = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_FullCircle)}");
            Particle_FullCircleHard = new Tex2DWithPath($"{Path_Particle}{nameof(Particle_FullCircleHard)}");

            Texture_BloomShockwave = new Tex2DWithPath($"{Path_General}{nameof(Texture_BloomShockwave)}");
            Texture_RarityGlow = new Tex2DWithPath($"{Path_General}{nameof(Texture_RarityGlow)}");

        }
        public override void Unload()
        {
            InvisAsset = null;

            Particle_ShinyOrb = null;
            Particle_HRShinyOrbSmall = null;
            Particle_Leafs = null;
            Particle_CrossGlow = null;
            Particle_Petal = null;
            Particle_NoaButterfly = null;
            Particle_FusableBall = null;
            Particle_FullCircle = null;
            Particle_FullCircleHard = null;

            Texture_BloomShockwave = null;
            Texture_RarityGlow = null;
        }
    }
}

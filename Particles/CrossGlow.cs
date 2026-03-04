using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Core.ParticleSystem;
using System;
using Terraria;

namespace SakurabaEmaMod.Particles
{
    public class CrossGlow : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        public float BeginScale;
        public SpriteEffects se = SpriteEffects.None;
        public bool UseFadeIn = true;
        private bool NoLighting;
        public CrossGlow(Vector2 position, Color color, int lifetime, float opacity, float scale, bool noLighting= true)
        {
            Position = position;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale = scale;
            BeginScale = scale;
            NoLighting = noLighting;
        }
        public CrossGlow(Vector2 position, Color color, int lifetime, float opacity, float scale, bool useFadeIn, bool noLighting = true)
        {
            Position = position;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale = scale;
            BeginScale = scale;
            UseFadeIn = useFadeIn;
            NoLighting = noLighting;
        }
        public override void OnSpawn()
        {
            if (Main.rand.NextBool())
                se = SpriteEffects.FlipHorizontally;

            if (UseFadeIn)
                Scale = BeginScale;
        }
        public override void Update()
        {
            if(!NoLighting && !ManosabaClientConfig.Instance.ParticleDontEmitLight)
            {
                Lighting.AddLight(Position, new Vector3(DrawColor.R / 255f, DrawColor.G /255f, DrawColor.B/255f));
            }
            if (LifetimeRatio < 0.5f)
            {
                if (UseFadeIn)
                    Scale = Lerp(0f, BeginScale, EaseOutCubic(LifetimeRatio * 2));
            }
            else
            {
                float progress = LifetimeRatio - 0.5f;
                Scale = Lerp(BeginScale, 0f, EaseOutCubic(progress * 2));
            }
        }
        // 这里采样没有问题，他贴图就是这样
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ManosabaTexture.Particle_CrossGlow.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, se, 0f);
        }
    }
}

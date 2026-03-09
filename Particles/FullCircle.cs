using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Core.ParticleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SakurabaEmaMod.Particles
{
    public class FullCircle: BaseParticle
    {
        public int BlendStateType;
        public override int UseBlendStateID => BlendStateType;
        public bool AffectedByGravity = false;
        public bool GlowCenter = true;
        public float FadeOut;
        public Color InitColor;
        public float GlowCenterScale = 0.5f;
        private bool NoLighting;
        private bool UseHardEdge;
        private float BeginScale = 0f;
        public FullCircle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, bool noLighting, bool useHardEdge = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = BeginScale= scale;
            BlendStateType = BlendStateID.Alpha;
            FadeOut = 1f;
            NoLighting = noLighting;
            UseHardEdge = useHardEdge;
        }
        public FullCircle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, bool noLighting, int blendStateType, bool useHardEdge = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = BeginScale =scale;
            BlendStateType = blendStateType;
            FadeOut = 1f;
            NoLighting = noLighting;
            UseHardEdge = useHardEdge;
        }

        public override void Update()
        {
            FadeOut -= 0.05f;
            if (LifetimeRatio < 0.5f)
            {
                Scale = Lerp(0f, BeginScale, EaseOutCubic(LifetimeRatio * 2));
            }
            else
            {
                float progress = LifetimeRatio - 0.5f;
                Scale = Lerp(BeginScale, 0f, EaseOutCubic(progress * 2));
            }
            //Scale *= 0.93f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            if (!NoLighting && !ManosabaClientConfig.Instance.ParticleDontEmitLight)
                Lighting.AddLight(Position, new Vector3(DrawColor.R / 255f, DrawColor.G /255f, DrawColor.B/255f));

            Velocity *= 0.95f;
            Rotation = Velocity.ToRotation() + PiOver2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = ManosabaTexture.Particle_FullCircle.Value;
            if (UseHardEdge)
                texture = ManosabaTexture.Particle_FullCircle.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
        }
    }

}

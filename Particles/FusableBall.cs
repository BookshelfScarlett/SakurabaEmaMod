using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.ParticleSystem;
using Terraria;

namespace SakurabaEmaMod.Particles
{
    public class FusableBall : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.NonPremult;
        private Vector2 Scale2;
        public FusableBall(Vector2 position, Vector2 velocity, Color color, int lifetime, float opacity, Vector2 scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale2 = scale;
            Important = true;
        }
        public override void OnSpawn()
        {
        }

        public override void Update()
        {
            Velocity *= 0.9f;
            Scale = Lerp(Scale, 0, EaseInCubic(LifetimeRatio));
            Rotation = Velocity.ToRotation();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ManosabaTexture.Particle_FusableBall.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale * Scale2, SpriteEffects.None, 0);
        }
    }
}

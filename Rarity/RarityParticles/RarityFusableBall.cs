using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using Terraria;

namespace SakurabaEmaMod.Rarity.RarityParticles
{

    public class RarityFusableBall : RaritySparkle
    {
        private Vector2 Scale2;
        public RarityFusableBall(Vector2 position, Vector2 velocity, Color color, int lifetime, float opacity, Vector2 scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale2 = scale;
        }
        public override void CustomUpdate()
        {
            Velocity *= 0.97f;
            Position += Velocity;
            Scale = Lerp(Scale, 0, EaseInCubic(LifetimeRatio));
            Rotation = Velocity.ToRotation();
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 drawPos)
        {
            Texture2D texture = ManosabaTexture.Particle_FusableBall.Value;
            spriteBatch.Draw(texture, drawPos, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale * Scale2, SpriteEffects.None, 0);
        }
    }
}

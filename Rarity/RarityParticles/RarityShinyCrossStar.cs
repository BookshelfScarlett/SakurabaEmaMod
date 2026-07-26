using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.ParticleSystem;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using System;
using Terraria;

namespace SakurabaEmaMod.Rarity.RarityParticles
{
    public class RarityShinyCrossStar : RaritySparkle
    {
        public bool UseRot = false;
        public override int UseBlendStateID => BlendStateID.Additive;
        public Color InitColor;
        public float SpinSpeed = 0;
        private float BeginScale;
        public RarityShinyCrossStar(Vector2 position, Vector2 velocity, Color color, int lifetime, float Rot, float opacity, float scale, float spinSpeed = 0f)
        {
            Position = position;
            Velocity = velocity;
            InitColor = color;
            Lifetime = lifetime;
            Rotation = Rot;
            Opacity = opacity;
            Scale = BeginScale = scale;
            SpinSpeed = spinSpeed;
        }
        public override void CustomUpdate()
        {
            Scale *= 0.93f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            Velocity *= 0.95f;
            Rotation += SpinSpeed;
            Position += Velocity;
            //太小的情况下直接处死粒子就行了
            if (Scale < BeginScale * 0.15f)
                Time = Lifetime;
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 drawPos)
        {
            Texture2D star = ManosabaTexture.Particle_SharpTear;
            for (float i = 0; i < 1f; i += 0.1f)
            {
                Vector2 starScale = GetScale(i);
                float colorAlpha = GetAlphaFade(1 - i);
                spriteBatch.Draw(star, drawPos, null, DrawColor * Opacity * colorAlpha, Rotation, star.Size() / 2, starScale * Scale, SpriteEffects.None, 0);
                spriteBatch.Draw(star, drawPos, null, DrawColor * Opacity * colorAlpha, Rotation + PiOver2, star.Size() / 2, starScale * Scale, SpriteEffects.None, 0);
                spriteBatch.Draw(star, drawPos, null, Color.White * Opacity, Rotation + PiOver2, star.Size() / 2, starScale * Scale * 0.5f, SpriteEffects.None, 0);
            }
        }
        public float GetAlphaFade(float t)
        {
            return Lerp(0.5f, 1f, t);
        }
        public Vector2 GetScale(float t)
        {
            Vector2 starScale = new(1.2f, 0.8f);
            Vector2 beginScale = new(0.2f, 0.05f);
            return Vector2.Lerp(beginScale, starScale, t);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SakurabaEmaMod.Rarity.RarityParticles
{
    public class RarityNoaButterfly : RaritySparkle
    {
        private float Speed = 5f;
        private int SeedOffset = 0;
        private float BeginScale = 1f;
        private float TurBulenceDirection = 0f;

        public RarityNoaButterfly(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifeTime;
            Scale = scale;
        }

        public override void CustomUpdate()
        {

            if (Speed != 0)
            {
                Vector2 idealVelocity = -Vector2.UnitY.RotatedBy(Lerp(-TurBulenceDirection, TurBulenceDirection, (float)Math.Sin(Time / 36f + SeedOffset) * 0.5f + 0.5f)) * Speed;
                float movementInterpolant = Lerp(0.01f, 0.25f, Utils.GetLerpValue(0, Lifetime / 2, Time, true));
                Velocity = Vector2.Lerp(Velocity, idealVelocity, movementInterpolant);
                Velocity = Velocity.SafeNormalize(-Vector2.UnitY) * Speed;
            }
            Position += Velocity;
            Velocity *= 0.9f;
            Scale = Lerp(BeginScale, 0, EaseOutCubic(LifetimeRatio));
            //Scale大于0.45的时候把TimeLeft的值直接设置为……Time的大小，以处死射弹
            if (Scale <= BeginScale * 0.10f)
                Time = Lifetime;
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = ManosabaTexture.Particle_ShinyOrb.Value;
            spriteBatch.Draw(texture, drawPosition, null, DrawColor, 0, texture.Size() * 0.5f, scale, 0, 0f);
            spriteBatch.Draw(texture, drawPosition, null, Color.White, 0, texture.Size() * 0.5f, scale * 0.5f, 0, 0f);
        }
    }
}

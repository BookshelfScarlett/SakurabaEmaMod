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
    public class RarityFullCircle : RaritySparkle
    {
        public int BlendStateType;
        public override int UseBlendStateID => BlendStateType;
        public float FadeOut;
        public Color InitColor;
        private bool UseHardCircle;
        public RarityFullCircle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, bool useHardCircle= false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
            FadeOut = 1f;
            UseHardCircle = useHardCircle;
        }

        public override void CustomUpdate()
        {
            FadeOut -= 0.015f;
            Scale *= 0.99f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            //减速需要更快。
            Velocity *= 0.975f;
            Position += Velocity;
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = UseHardCircle ? ManosabaTexture.Particle_FullCircleHard.Value : ManosabaTexture.Particle_FullCircle.Value;
            spriteBatch.Draw(texture, drawPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
        }
    }
}

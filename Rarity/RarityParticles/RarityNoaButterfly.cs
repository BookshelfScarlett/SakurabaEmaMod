using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Configs;
using SakurabaEmaMod.Core.ParticleSystem;
using SakurabaEmaMod.Particles;
using SakurabaEmaMod.Rarity.RarityDrawHandler;
using System;
using Terraria;

namespace SakurabaEmaMod.Rarity.RarityParticles
{
    public class RarityNoaButterfly : RaritySparkle
    {
        private Vector2 BeginVector;
        private float Speed;
        private float SeedValue;
        //控制摆动/浮动
        private float HorizonalFloting;
        private float VerticalFloating;
        //振翅速度
        private float WingFlapSpeed;
        //用于关联帧动画
        private float swingOffsetX;
        private bool NoLighting;
        private bool DrawGlowingOrbParticle;
        public RarityNoaButterfly(Vector2 position, Vector2 beginVector, Color color, int lifeTime, float opacity, float scale, float speed, bool noLighting = false, bool drawGlowingOrbParticle = false)
        {
            Position = position;
            BeginVector = beginVector;
            DrawColor = color;
            Opacity = opacity;
            Lifetime = lifeTime;
            Scale = scale;
            Speed = speed;
            HorizonalFloting = Main.rand.NextFloat(0.5f, 1.2f);
            VerticalFloating = Main.rand.NextFloat(0.3f, 0.8f);
            WingFlapSpeed = Main.rand.NextFloat(1.8f, 2.5f);
            NoLighting = noLighting;
            DrawGlowingOrbParticle = drawGlowingOrbParticle;
            SeedValue = Main.rand.Next(0, 100000);
        }
        public override void CustomUpdate()
        {
            if (!NoLighting && !ManosabaClientConfig.Instance.ParticleDontEmitLight)
                Lighting.AddLight(Position, new Vector3(DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f));
            if (Speed != 0)
            {

                //基础移动
                float currentSpeed = Speed * (1 - LifetimeRatio); // 后期减速70%
                Vector2 baseMovement = BeginVector.SafeNormalize(Vector2.One) * currentSpeed;

                //横向浮动
                swingOffsetX = (float)Math.Sin(Time * 0.05f + SeedValue * 0.0001f) * HorizonalFloting;

                //纵向浮动
                float floatOffsetY = (float)Math.Cos(Time * 0.07f + SeedValue * 0.0002f) * VerticalFloating;

                //叠加速度
                Velocity = baseMovement + new Vector2(swingOffsetX, floatOffsetY);

                //轻微旋转：随摆动调整角度，增强真实感
                Rotation = BeginVector.ToRotation() + swingOffsetX * 0.02f;
                //每100帧有概率变向
                if (Main.rand.NextBool(100))
                    BeginVector = BeginVector.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));

                //透明度随生命周期渐变
                Opacity = LifetimeRatio < 0.1f ? Lerp(0f, 1, Opacity) : (LifetimeRatio > 0.9f ? (1 - LifetimeRatio) * 10 : 1f);
                Position += Velocity;
            }
            Scale *= 0.98f;
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 pos)
        {
            Texture2D texture = ManosabaTexture.Particle_NoaButterfly.Value;

            float flapProgress = (LifetimeRatio * WingFlapSpeed + Math.Abs(swingOffsetX) * 0.5f + SeedValue * 0.00001f) % 1f;
            int frameindex = (int)(flapProgress * 4);
            Rectangle frame = ManosabaTexture.Particle_NoaButterfly.Texture.Frame(2, 2, frameindex % 2, frameindex / 2);
            Vector2 origin = frame.Size() * 0.5f;

            spriteBatch.Draw(texture, pos, frame, DrawColor * Opacity, Rotation + ToRadians(95f), origin, Scale, 0, 0f);
        }
    }
}

using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Particles;
using System.Threading;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Players
{
    /// <summary>
    /// 需注意的是，由于把人物特效全都扔到了一个统一的玩家类里进行管理了
    /// 因此这里需要一定程度上严格的命名规范
    /// </summary>
    public partial class ManosabaPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            if (ParticleTimer > 0)
                ParticleTimer--;
        }
        public void DrawParticleOnNeed()
        {
            switch (ManosabaGirl)
            {
                case ManosabaGirlID.NatsumeAnan:
                    if (Player.IsStandingStil(5f))
                        DrawAnanStandingParticle();
                    else
                        DrawAnanParticle();
                    break;
            }
        }

        #region 夏目安安特效
        public void DrawAnanStandingParticle()
        {
            if (ParticleTimer <= 0f)
            {
                //不用在脚底下生成。在人物周围尝试
                //当我没说
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                spawnPos = Player.Center + Vector2.UnitY * 25f;
                for (int i = 0; i < 4; i++)
                {
                    //花瓣与光球
                    Vector2 vec = (-Vector2.UnitY).RotatedBy(Main.rand.NextFloat(ToRadians(10f)) * Main.rand.NextBool().ToDirectionInt());
                    spawnPos += Vector2.UnitX * Main.rand.NextFloat(-5f, 6f);
                    Vector2 setpos = spawnPos;
                    Color setColor = RandLerpColor(Color.DeepSkyBlue, Color.White);
                    new NoaButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f,drawGlowingOrbParticle:true).Spawn();
                    new CrossGlow(setpos, setColor, 20, 1, 0.05f, false, false).SpawnToPriority();
                }
                ParticleTimer = 120f;
            }
        }
        public void DrawAnanParticle()
        {
            //如果玩家速度过小，我们不生成粒子。
            Vector2 mountedPlayerPos = Player.Center;
            Rectangle rec = Utils.CenteredRectangle(mountedPlayerPos, new Vector2(Player.width, Player.height));
            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(rec);
            Vector2 butterFly = Player.velocity.SafeNormalize(Vector2.UnitX).RotatedByRandom(PiOver2);
            if (Main.rand.NextBool())
                new TurbulenceShinyOrb(spawnPos - butterFly * Main.rand.NextFloat(20, 30f), 0.9f, RandLerpColor(Color.DeepSkyBlue, Color.White), 80, 0.34f, RandRotTwoPi).Spawn();
            if (Main.rand.NextBool(8))
            {
                spawnPos = spawnPos - butterFly * Main.rand.NextFloat(20f, 30f);
                new NoaButterfly(spawnPos, butterFly, RandLerpColor(Color.White, Color.DeepSkyBlue), 80, 1f, 0.20f, 0.8f).SpawnToPriorityNonPreMult();
            }
        }
        #endregion

    }
}

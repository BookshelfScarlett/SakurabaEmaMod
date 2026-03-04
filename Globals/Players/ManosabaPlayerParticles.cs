using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Particles;
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
                case ManosabaGirlID.NikaidouHiro:
                    if (Player.IsStandingStil(5f))
                        DrawHiroStandingParticle();
                    else
                        DrawHiroParticle();
                    break;

            }
        }
        #region 二阶堂希罗特效
        public void DrawHiroStandingParticle()
        {
            if (ParticleTimer > 0)
                return;
            //这里的写法潜在问题是如果有人试图手动操作玩家的贴图大小，则无法校准
            //但是……我还真没见过这种情况。先不管反正。
            //顺带一提这里的效果故意与艾玛复用。
            Vector2 spawnPos = Player.direction > 0 ? new Vector2(Player.position.X- 5, Player.position.Y + 2) : new Vector2(Player.position.X + 20, Player.position.Y + 2);
            new CrossGlow(spawnPos, Color.Pink, 30, 1, 0.10f).Spawn();
            for (int i = 0; i < 3; i++)
            {
                //花瓣与光球
                Vector2 vel = Vector2.UnitY.RotateRandom(PiOver4) * Main.rand.NextFloat(2f);
                new ShinyOrbParticle(spawnPos, vel, RandLerpColor(Color.Crimson, Color.DarkRed), 40, 0.64f, false).Spawn();
                new ShinyOrbParticle(spawnPos, vel, Color.White, 40, 0.64f * 0.5f, false).Spawn();
                new Petal(spawnPos, Vector2.UnitY * Main.rand.NextFloat(1.1f, 1.3f), RandLerpColor(Color.Crimson, Color.DarkRed), 120, RandRotTwoPi, 0.8f, Main.rand.NextFloat(0.08f, 0.1f), 0.3f).Spawn();
            }
            ParticleTimer = 120f;
        }
        public void DrawHiroParticle()
        {
            //如果玩家速度过小，我们不生成粒子。
            Vector2 mountedPlayerPos = Player.Center;
            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(mountedPlayerPos,new(30)));
            Vector2 vel = Player.velocity.SafeNormalize(Vector2.UnitX) * -Main.rand.NextFloat(0.3f, 1.25f) * 1.1f;
            //需往后移一些
            spawnPos += vel.SafeNormalize(Vector2.UnitX) * 15f;
            if (Main.rand.NextBool(4))
            {
                new ShinyOrbParticle(spawnPos, vel, RandLerpColor(Color.Crimson, Color.DarkRed), 40, 0.64f, false).Spawn();
                new ShinyOrbParticle(spawnPos, vel, Color.White, 40, 0.64f * 0.5f, false).Spawn();
            }
            if (Main.rand.NextBool(3))
                new Petal(spawnPos, vel, RandLerpColor(Color.Crimson, Color.DarkRed), 40, RandRotTwoPi, 1f, 0.1f, 0.5f).Spawn();

        }
        #endregion

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

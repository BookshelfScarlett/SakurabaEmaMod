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
            bool isStanding = Player.IsStandingStil(5f);
            switch (ManosabaGirl)
            {
                case ManosabaGirlID.TachibanaSherry:
                    DrawSherryParticle();
                    break;
                case ManosabaGirlID.SakurabaEma:
                    DrawEmaParticle();
                    break;
                case ManosabaGirlID.NatsumeAnan:
                    if (isStanding)
                        DrawAnanStandingParticle();
                    else
                        DrawAnanParticle();
                    break;
                case ManosabaGirlID.NikaidouHiro:
                    if (isStanding)
                        DrawHiroStandingParticle();
                    else
                        DrawHiroParticle();
                    break;
                case ManosabaGirlID.JougasakiNoah:
                    if (isStanding)
                        DrawNoahStandingParticle();
                    else
                        DrawNoahParticle();
                    break;

            }
        }
        #region 橘雪莉
        public void DrawSherryParticle()
        {
            if (Player.IsStandingStil(5f) && Player.velocity.Y == 0)
                DrawSherryStandingParticle();
            else
                DrawSherryMovingParticle();
        }
        public void DrawSherryMovingParticle()
        {
            if (Main.rand.NextBool())
                return; 
            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new(Player.width, Player.height)));
            Vector2 vel = Player.velocity.SafeNormalize(Vector2.UnitX).RotatedByRandom(PiOver4) * -Main.rand.NextFloat(1,6f);
            Color randColor = RandLerpColor(Color.RoyalBlue, Color.SkyBlue);
            new TurbulenceGlowOrb(spawnPos - Player.velocity.SafeNormalize(Vector2.UnitX) * Main.rand.NextFloat(2f, 2.9f), 0.82f, randColor, 120, 0.14f * Main.rand.NextFloat(0.8f,1.121f), vel.ToRotation()).Spawn();
            if (ParticleTimer > 0)
                return;
            ParticleTimer = 60f;
            new KiraStar(spawnPos, vel / 4f, randColor, 120, vel.ToRotation(), 1,0.45f,fadeIn:true).Spawn();
        }
        public void DrawSherryStandingParticle()
        {
            Vector2 spawnPos = Player.Center + Vector2.UnitY * (Player.height / 2 + 1) + Vector2.UnitX * Main.rand.NextFloat(8.7f) * Main.rand.NextBool().ToDirectionInt();
            Vector2 vel = -Vector2.UnitY * Main.rand.NextFloat();
            Vector2 shapeVel = Vector2.UnitX * Main.rand.NextFloat() * Main.rand.NextBool().ToDirectionInt();
            new ShinyOrbParticle(spawnPos, vel, RandLerpColor(Color.SkyBlue, Color.RoyalBlue), 40, 0.16f, false).Spawn();
            new StarShape(spawnPos, shapeVel, RandLerpColor(Color.SkyBlue, Color.RoyalBlue), 0.18f, 40).Spawn();
        }

        #endregion
        #region 樱羽艾玛

        public void DrawEmaParticle()
        {
            if (Player.IsStandingStil(5f))
                DrawEmaStandingParticle();
            else
                DrawEmaMovingParticle();
        }
        public void DrawEmaMovingParticle()
        {
            //如果玩家速度过小，我们不生成粒子。
            if (Main.rand.NextBool())
            {
                Vector2 mountedPlayerPos = Player.position;
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(new Rectangle((int)mountedPlayerPos.X, (int)mountedPlayerPos.Y, Player.width, Player.height));
                Vector2 vel = Player.velocity.SafeNormalize(Vector2.UnitX) * -Main.rand.NextFloat(0.3f, 1.25f) * 1.1f;
                new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(3), 1f, RandLerpColor(Color.HotPink, Color.LightPink), 40, 0.34f, RandRotTwoPi).Spawn();
                if (Main.rand.NextBool(3))
                    new Petal(spawnPos, vel, RandLerpColor(Color.HotPink, Color.LightPink), 40, RandRotTwoPi, 1f, 0.1f, 0.5f).Spawn();
            }
        }
        public void DrawEmaStandingParticle()
        {
            //这里的写法潜在问题是如果有人试图手动操作玩家的贴图大小，则无法校准
            //但是……我还真没见过这种情况。先不管反正。
            if (ParticleTimer > 0)
                return;

            ParticleTimer = 120f;
            Vector2 spawnPos = Player.direction > 0 ? new Vector2(Player.position.X + 2, Player.position.Y + 2) : new Vector2(Player.position.X + 15, Player.position.Y + 2);
            new CrossGlow(spawnPos, Color.Pink, 30, 1, 0.10f).Spawn();
            for (int i = 0; i < 3; i++)
            {
                //花瓣与光球
                new Petal(spawnPos, Vector2.UnitY * Main.rand.NextFloat(1.1f, 1.3f), RandLerpColor(Color.HotPink, Color.LightPink), 120, RandRotTwoPi, 0.8f, Main.rand.NextFloat(0.08f, 0.1f), 0.3f).Spawn();
                new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(3), 0.2f, RandLerpColor(Color.HotPink, Color.LightPink), 120, 0.22f, RandRotTwoPi).Spawn();
            }

        }
        #endregion
        #region 城崎诺亚
        public void DrawNoahStandingParticle()
        {
            if (Main.rand.NextBool(32))
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center - Vector2.UnitY * 30f, new(Player.width, Player.height - 30f)));
                Vector2 vel = Vector2.UnitY.RotateRandom(PiOver4) * Main.rand.NextFloat(2f);
                Color[] randColor = [Color.Red, Color.SkyBlue, Color.Yellow, Color.Green, Color.White];
                Color drawColor = Utils.SelectRandom(Main.rand, randColor);
                new FullCircle(spawnPos, vel + Vector2.UnitY * Main.rand.NextFloat(1, 3) * 0.83f, drawColor, 120, 0.08f, true).Spawn();
            }
        }
        public void DrawNoahParticle()
        {
            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new(Player.width, Player.height)));
            Vector2 vel = Player.velocity.SafeNormalize(Vector2.UnitX) * -Main.rand.NextFloat(4f);
            Color[] randColor = [Color.Red, Color.SkyBlue, Color.Yellow, Color.Green, Color.White];
            Color drawColor = Utils.SelectRandom(Main.rand, randColor);
            new FullCircle(spawnPos - vel, vel+ Vector2.UnitY * Main.rand.NextFloat(1, 3) * 0.5f, drawColor, 80, 0.09f * Main.rand.NextFloat(0.4f, 1.1f), true).Spawn();
        }

        #endregion
        #region 二阶堂希罗
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
        #region 夏目安安
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
                    new NoahButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f,drawGlowingOrbParticle:true).Spawn();
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
                new NoahButterfly(spawnPos, butterFly, RandLerpColor(Color.White, Color.DeepSkyBlue), 80, 1f, 0.20f, 0.8f).SpawnToPriorityNonPreMult();
            }
        }
        #endregion

    }
}

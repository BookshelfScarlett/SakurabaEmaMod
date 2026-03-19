using Microsoft.Xna.Framework;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.NetCodes;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Particles;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Players
{
    public partial class ManosabaPlayer : ModPlayer
    {
        public List<short> CharactorList = [ManosabaGirlID.NikaidouHiro, ManosabaGirlID.NatsumeAnan, ManosabaGirlID.JougasakiNoah];
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            SerCharactorPreKill(ref playSound, ref genDust);
            //是的这里会先预制默认为否
            //后面会正确处理其操作
            if (NoaButterflyDeath)
            {
                ModifyNoaMagic();
                return true;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }

        private void SerCharactorPreKill(ref bool playSound, ref bool genDust)
        {
            if (ManosabaGirl == ManosabaGirlID.None)
                return;
            //playSound = false;
            genDust = false;
            switch (ManosabaGirl)
            {
                case ManosabaGirlID.SakurabaEma:
                    DrawEmaKillParticles();
                    EmaDeathSound();
                    break;
                case ManosabaGirlID.NikaidouHiro:
                    DrawHiroKillParticles();
                    HiroDeathSound();
                    break;
                case ManosabaGirlID.NatsumeAnan:
                    DrawAnanKillParticles();
                    AnanDeathSound();
                    break;
                
                case ManosabaGirlID.JougasakiNoah:
                    DrawNoahKillParticles();
                    NoahDeathSound();
                    break;
                default:
                    break;
            }
            return;
        }

        public void CustomSoundOnHurt(Player.HurtInfo hurtInfo)
        {
            if (!DisableOrignalSound)
                return;
            
            hurtInfo.SoundDisabled = true;
            //代办：其余角色的承伤音效
            switch (ManosabaGirl)
            {
                case ManosabaGirlID.SakurabaEma:
                    EmaPlayerSound();
                    break;
                case ManosabaGirlID.NatsumeAnan:
                    AnanPlayerSound();
                    break;
                case ManosabaGirlID.NikaidouHiro:
                    HiroPlayerSound();
                    break;
                case ManosabaGirlID.JougasakiNoah:
                    NoahPlayerSound();
                    break;
            }
        }

        

        #region 艾玛
        //出于某些原因如果真的有神人玩家选择加载了樱羽艾玛死亡音效mod，则禁用这个物品的所有自定义音效
        //还有这里的中文名纯故意的
        private bool 草艾玛
        {
            get
            {
                return ModLoader.HasMod("Sounds_SakurabaEma");
            }
        }
        private void DrawEmaKillParticles()
        {
            //落樱和散发粒子，修改为这些。
            new CrossGlow(Player.Center, Color.Pink, 30, 1f, 0.13f, false).Spawn();
            for (int i = 0; i < 8; i++)
            {
                Vector2 spawnPos = Player.Center.ToRandCirclePos(36f);
                new Petal(spawnPos, Vector2.UnitY * Main.rand.NextFloat(1.1f, 1.3f), RandLerpColor(Color.HotPink, Color.LightPink).ToAddColor(50), 120, RandRotTwoPi, 0.8f, Main.rand.NextFloat(0.1f, 0.12f), 0.3f).Spawn();
            }
            for (int i = 0; i < 25; i++)
            {
                Vector2 spawnPos = Player.Center.ToRandCirclePos(36f);
                new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(4), 1.2f, RandLerpColor(Color.HotPink, Color.LightPink), 120, 0.42f, RandRotTwoPi).Spawn();

            }
        }
        private void EmaDeathSound()
        {
            if (!草艾玛)
                return;
            SoundStyle sound = EmaKiangSound ? ManosabaSounds.Ema_Kiang : ManosabaSounds.Ema_HitHeavy;
            SoundEngine.PlaySound(sound, Player.Center);
        }
        private void EmaPlayerSound()
        {
            if (!草艾玛)
                return;
            //kiang
            if (Main.netMode == NetmodeID.Server)
            {
                int soundType;
                if (EmaKiangSound)
                {
                    //是的，这里漂洋过海半天就是为了做包的收发。
                    soundType = 1;
                    SoundPackets.BroadcastSound(Player.Center, soundType);
                }
                else
                {
                    soundType = Main.rand.NextBool().ToInt();
                    SoundPackets.BroadcastSound(Player.Center, soundType);
                }
            }
            else
            {
                if (EmaKiangSound)
                {
                    //是的，这里漂洋过海半天就是为了做包的收发。
                    SoundEngine.PlaySound(ManosabaSounds.Ema_Kiang, Player.Center);
                }
                else
                {
                    SoundStyle hitSound = Main.rand.NextBool() ? ManosabaSounds.Ema_HitSound : ManosabaSounds.Ema_Kiang;
                    SoundEngine.PlaySound(hitSound, Player.Center);
                }
            }
        }
        #endregion
        #region 诺亚
        private void DrawNoahKillParticles()
        {
            Color[] randColor = [Color.Red, Color.SkyBlue, Color.Yellow, Color.Green, Color.White];
            for (int i = 0; i < 18; i++)
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                Vector2 vel = RandVelTwoPi(6f);
                Color drawColor = Utils.SelectRandom(Main.rand, randColor);
                new FullCircle(spawnPos, vel, drawColor, 80, 0.1f * Main.rand.NextFloat(0.7f, 1.1f), true).Spawn();
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                Vector2 vel = Vector2.UnitY.RotateRandom(ToRadians(20f)) * -Main.rand.NextFloat(1f, 6f);
                Color drawColor = Utils.SelectRandom(Main.rand, randColor);
                new NoahButterfly(spawnPos, vel, drawColor, 120, 1, 0.2f, 1.2f, drawGlowingOrbParticle: true).Spawn();
            }
        }
        private void NoahDeathSound()
        {
            bool activeSpecialDeathSound = NoahCryIfHiroIsNearby();
            bool justCrying = AbsoluteNoahCryingCinemaButWhy(out SoundStyle crySound);
            SoundStyle deathSound = ManosabaSounds.Noah_Hit;
            //我tm也不知道谁要听这个7个诺亚大哭
            if (justCrying)
                deathSound = crySound;
            if (activeSpecialDeathSound)
                deathSound = ManosabaSounds.Noah_SpecialDeath;
            SoundEngine.PlaySound(deathSound, Player.Center);
        }
        /// <summary>
        /// 诺亚死亡时查看周围是否有希罗时装的人物
        /// 如果有，则以1/6的可能性让诺亚喊希罗语音。
        /// </summary>
        /// <returns></returns>
        private bool NoahCryIfHiroIsNearby()
        {
            if (!Main.dedServ)
                return false;
            foreach (var p in Main.ActivePlayers)
            {
                //统一管理的好处。
                bool activeHiro = p.team == Player.team && p.ManosabaMod().ManosabaGirl == ManosabaGirlID.NikaidouHiro;
                //必须得在可视范围。
                bool isCloseEnough = (p.Center - Player.Center).LengthSquared() < 600f * 600f && Collision.CanHit(p.Center, 1, 1, Player.Center, 1, 1);
                if (activeHiro && isCloseEnough && Main.rand.NextBool(5))
                {
                    //找到任意一个希罗就行了，我们立刻break出去
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// i 诺 T V
        /// </summary>
        /// <returns></returns>

        private bool AbsoluteNoahCryingCinemaButWhy(out SoundStyle crySound)
        {
            crySound = ManosabaSounds.Noah_Cry;
            return NoahCryDeath;
        }

        private void NoahPlayerSound()
        {
            SoundStyle deathSound = ManosabaSounds.Noah_Hit;
            SoundEngine.PlaySound(deathSound, Player.Center);
        }

        #endregion
        #region 希罗

        /// <summary>
        /// 从安安下方复用
        /// 但是修改了粒子逻辑
        /// </summary>
        private void DrawHiroKillParticles()
        {
            //落樱和散发粒子，修改为这些。
            for (int i = 0; i < 15; i++)
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                //花瓣与光球
                Vector2 vec = (-Vector2.UnitY).RotatedBy(Main.rand.NextFloat(ToRadians(10f)) * Main.rand.NextBool().ToDirectionInt());
                spawnPos += Vector2.UnitX * Main.rand.NextFloat(-5f, 6f);
                Vector2 setpos = spawnPos;
                Color setColor = RandLerpColor(Color.Red, Color.White);
                new NoahButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f).Spawn();
                spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                new Petal(spawnPos, -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(ToRadians(-30f), ToRadians(30f))), RandLerpColor(Color.DarkRed, Color.Crimson), 100, RandRotTwoPi, 1f, 0.1f, 1.2f).Spawn();
            }
        }
        /// <summary>
        /// 希罗死亡时查看诺亚是否在附近
        /// 如果有，则以1/6的可能性播报特殊的希罗死亡语音
        /// </summary>
        /// <returns></returns>
        private bool NoahCryIfHiroIsDeath()
        {
            if (!Main.dedServ)
                return false;
            foreach (var p in Main.ActivePlayers)
            {
                //统一管理的好处。
                bool activeHiro = p.team == Player.team && p.ManosabaMod().ManosabaGirl == ManosabaGirlID.JougasakiNoah;
                //必须得在可视范围。
                bool isCloseEnough = (p.Center - Player.Center).LengthSquared() < 600f * 600f && Collision.CanHit(p.Center, 1, 1, Player.Center, 1, 1);
                if (activeHiro && isCloseEnough && Main.rand.NextBool(5))
                {
                    //找到任意一个希罗就行了，我们立刻break出去
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 非常神经病的是原作希罗本来就没多少能用的惨叫
        /// 我也不大可能扔那个电梯惨叫过来
        /// 反正嗯算了
        /// 
        /// </summary>
        private void HiroDeathSound()
        {
            SoundStyle deathSound = Main.rand.NextBool() ? ManosabaSounds.Hiro_Death : ManosabaSounds.Hiro_Hit;
            if (NoahCryIfHiroIsDeath())
                deathSound = ManosabaSounds.Noah_SpecialCry;
            SoundEngine.PlaySound(deathSound, Player.Center);
        }

        private void HiroPlayerSound()
        {
            SoundStyle deathSound = ManosabaSounds.Hiro_Hit;
            SoundEngine.PlaySound(deathSound, Player.Center);
        }

        #endregion
        #region 安安
        private void DrawAnanKillParticles()
        {
            if (NoaButterflyDeath)
                return;
            //落樱和散发粒子，修改为这些。
            for (int i = 0; i < 15; i++)
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                //花瓣与光球
                Vector2 vec = (-Vector2.UnitY).RotatedBy(Main.rand.NextFloat(ToRadians(10f)) * Main.rand.NextBool().ToDirectionInt());
                spawnPos += Vector2.UnitX * Main.rand.NextFloat(-5f, 6f);
                Vector2 setpos = spawnPos;
                Color setColor = RandLerpColor(Color.DeepSkyBlue, Color.White);
                new NoahButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f).Spawn();
                spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                new TurbulenceShinyOrb(spawnPos, 1f, setColor, 120, 0.67f, RandRotTwoPi).Spawn();
            }
        }
        private void AnanDeathSound()
        {
            SoundStyle deathSound = ManosabaSounds.Anan_Death;
            SoundEngine.PlaySound(deathSound, Player.Center);
        }
        private void AnanPlayerSound()
        {
            SoundStyle hitSound = ManosabaSounds.Anan_Hit;
            SoundEngine.PlaySound(hitSound, Player.Center);
        }
        #endregion
        /// <summary>
        /// 蝴蝶。
        /// 不过即使如此，角色自己的专属死亡特效可能也会被替换为蝴蝶
        /// 虽然放在这里也只是因为原作有这个东西
        /// </summary>
        public void ModifyNoaMagic()
        {

        }

        #region 给复写钩子用的超级方法合集。
        public void ModifyCustomSound(ref Player.HurtModifiers modifiers)
        {
            if (DisableOrignalSound)
            {
                modifiers.DisableSound();
            }
        }
        public bool DisableOrignalSound
        {
            get
            {
                return ManosabaGirl != ManosabaGirlID.None;
            }
        }

        #endregion
        #region 一大堆复写，扔到了最下面，因为本质上是复制粘贴
        public override void ModifyHurt(ref Player.HurtModifiers modifiers) => ModifyCustomSound(ref modifiers);
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) => ModifyCustomSound(ref modifiers);
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) => ModifyCustomSound(ref modifiers);
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) => CustomSoundOnHurt(hurtInfo);
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) => CustomSoundOnHurt(hurtInfo);
        public override void OnHurt(Player.HurtInfo info) => CustomSoundOnHurt(info);
        #endregion

    }
}

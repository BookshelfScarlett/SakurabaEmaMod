using Microsoft.Xna.Framework;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Menus.MainMenu;
using SakurabaEmaMod.Particles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Players
{
    public partial class ManosabaPlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            SerCharactorPreKill(ref playSound, ref genDust);
            //是的这里会先预制默认为否
            //后面会正确处理其操作
            if(NoaButterflyDeath)
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
                case ManosabaGirlID.NatsumeAnan:
                    DrawAnanKillParticles();
                    AnanDeathSound();
                    break;
                case ManosabaGirlID.NikaidouHiro:
                    DrawHiroKillParticles();
                    HiroDeathSound();
                    break;
                default:
                    break;
            }
            return;
        }
        #region 希罗
        /// <summary>
        /// 非常神经病的是原作希罗本来就没多少能用的惨叫
        /// 我也不大可能扔那个电梯惨叫过来
        /// 反正嗯算了
        /// 
        /// </summary>
        private void HiroDeathSound()
        {
        }

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
                new NoaButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f).Spawn();
                spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                new Petal(spawnPos, -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(ToRadians(-30f), ToRadians(30f))), RandLerpColor(Color.DarkRed, Color.Crimson), 100, RandRotTwoPi, 1f, 0.1f, 1.2f).Spawn();
            }

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
                new NoaButterfly(setpos, vec, setColor, 80, 1f, 0.14f, 0.8f).Spawn();
                spawnPos = Main.rand.NextVector2FromRectangle(Utils.CenteredRectangle(Player.Center, new Vector2(Player.width, Player.height)));
                new TurbulenceShinyOrb(spawnPos, 1f, setColor, 120, 0.67f, RandRotTwoPi).Spawn();
            }
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
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            ModifyCustomSound(ref modifiers);
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            ModifyCustomSound(ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            ModifyCustomSound(ref modifiers);
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            CustomSoundOnHurt(hurtInfo);
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            CustomSoundOnHurt(hurtInfo);
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            CustomSoundOnHurt(info);
        }
        public void CustomSoundOnHurt(Player.HurtInfo hurtInfo)
        {
            if (DisableOrignalSound)
            {
                hurtInfo.SoundDisabled = true;
                //代办：其余角色的承伤音效
                switch (ManosabaGirl)
                {
                    case ManosabaGirlID.NatsumeAnan:
                        AnanPlayerSound();
                        break;
                }
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

        public void ModifyCustomSound(ref  Player.HurtModifiers modifiers)
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

    }
}

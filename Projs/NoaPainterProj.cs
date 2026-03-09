using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Globals.Class;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Globals.Handlers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Projs
{
    public class NoaPainterProj :  CharactorProj
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override short SetCharactor => ManosabaGirlID.JougasakiNoah;
        public override string Texture => $"SakurabaEmaMod/Assets/Texture/Projs/NoaPainter";
        public AnimationHandler PainterAni = new(10);
        public bool Init = false;
        public Vector2 InitMouseVec = Vector2.Zero;
        public float MouseVecRot = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public Vector2 Offset => new Vector2(28f, 0f);
        public ref float UseTimer => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            //手持射弹非常麻烦，但还好。
            //反正得注册一大堆吧
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.noEnchantments = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 10000;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            if (!Init)
            {

            }
            bool stillInUse = (Owner.channel || Owner.controlUseTile) && !Owner.noItems && !Owner.CCed;
            if (stillInUse)
            {
                UpdateProjData();
                HoldoutAI();
            }
            else
            {
                Projectile.Kill();
            }
        }
        public void InitAI()
        {
            PainterAni.MaxAniProgress[AnimationHandler.AniBegin] = 15;
            PainterAni.MaxAniProgress[AnimationHandler.AniEnd] = 15;
            InitMouseVec = (Main.MouseWorld - Owner.Center).SafeNormalize(Vector2.UnitX);
            MouseVecRot = InitMouseVec.ToRotation();
            
        }

        private void UpdateProjData()
        {
            Projectile.Center = Owner.Center + Offset.RotatedBy(Projectile.rotation);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.ChangeDir((Main.MouseWorld.X > Owner.Center.X).ToDirectionInt());
            Vector2 HeldAimPoint = new Vector2(10 , 0f).RotatedBy(Projectile.rotation);
            float ArmRot = (Projectile.Center + HeldAimPoint - Owner.Center).ToRotation();
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, ArmRot - PiOver2);
        }
        private void HoldoutAI()
        {
            MouseVecRot = Utils.AngleLerp(MouseVecRot, (Main.MouseWorld - Owner.Center).ToRotation(), 0.2f);
            Projectile.velocity = Projectile.rotation.ToRotationVector2();
            DoAnimation();
        }
        public void DoAnimation()
        {
            if (!PainterAni.HasFinish[AnimationHandler.AniBegin])
            {
                DoBeginAnimation();

                if (PainterAni.AniProgress[AnimationHandler.AniBegin] < PainterAni.MaxAniProgress[AnimationHandler.AniBegin])
                    PainterAni.AniProgress[AnimationHandler.AniBegin]++;
                if (PainterAni.AniProgress[AnimationHandler.AniBegin] >= PainterAni.MaxAniProgress[AnimationHandler.AniBegin])
                {
                    PainterAni.HasFinish[AnimationHandler.AniBegin] = true;
                    Projectile.netUpdate = true;
                }
            }
            else if(!PainterAni.HasFinish[AnimationHandler.AniEnd])
            {
                if (PainterAni.AniProgress[AnimationHandler.AniEnd] < PainterAni.MaxAniProgress[AnimationHandler.AniEnd])
                    PainterAni.AniProgress[AnimationHandler.AniEnd]++;

            }
        }

        private void DoBeginAnimation()
        {
            throw new NotImplementedException();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projTex = Request<Texture2D>(Texture).Value;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? PiOver2 + PiOver4 : PiOver4);
            SB.Draw(projTex, Projectile.Center - Main.screenPosition, null, Color.White, drawRotation, projTex.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}

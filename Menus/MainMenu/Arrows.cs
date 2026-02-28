using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.Audio;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class LeftArrow : ArrowClass
    {
        public override Vector2 Center => new Vector2(50f, Main.screenHeight /2f);
        public override bool IsLeftArrow => true;
        public override Texture2D ArrowTexture => ManosabaMenuAssets.Main_ArrowLeft.Texture.Value;
        public override Rectangle Hitbox => base.Hitbox;
    }
    public class RightArrow : ArrowClass
    {
        public override Vector2 Center => new Vector2(Main.screenWidth - 50f, Main.screenHeight /2f);
        public override bool IsLeftArrow => false;
        public override Texture2D ArrowTexture => ManosabaMenuAssets.Main_ArrowRight.Texture.Value;
        public override Rectangle Hitbox => base.Hitbox;
    }

    public abstract class ArrowClass : ManosabaHud
    {
        public virtual Vector2 Center => Vector2.Zero;
        public virtual bool IsLeftArrow => false;
        public virtual Rectangle Hitbox => Utils.CenteredRectangle(Center - Vector2.UnitX * IsLeftArrow.ToDirectionInt() * 20f, new Vector2(75, 1200));
        public virtual Texture2D ArrowBackground => ManosabaMenuAssets.Main_ArrowBack.Texture.Value;
        public virtual Texture2D ArrowTexture { get; }
        public sealed override void SetDefaults()
        {
            Rectangle = Hitbox;
            Position = Center;
            Opacity = 0;
        }
        public override void MouseHover(bool isHover)
        {
            //指针滑过的时候让Opacity上升，用于保证背景的透明化。
            if (isHover)
            {
                if (Main.mouseLeft || Main.mouseRight)
                {
                    Opacity = Lerp(Opacity, 0.95f, 0.2f);
                    Scale2 = Lerp(Scale2, 0.95f, 0.2f);
                }
                else
                {
                    Scale2 = Lerp(Scale2, 1.05f, 0.2f);
                    Opacity = Lerp(Opacity, 1.0f, 0.2f);
                }
            }
            else
            {
                Scale2 = Lerp(Scale2, 1f, 0.2f);
                Opacity = Lerp(Opacity, 0f, 0.2f);
            }

        }
        public override void OnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_Cancel);
            ManosabaBackground.CanChangeMenu = true;
            //按下的时候给背景上一层黑底，但是不影响其他地方的绘制。
            ManosabaBackground.BackgroundFading = 1;
            ref int BackID = ref ManosabaBackground.CurrentBackgroundID; 
            if (IsLeftArrow)
            {
                if (BackID <= 0)
                    BackID = ManosabaMenuID.Margo;
                else
                    BackID -= 1;

            }
            else
            {
                if (BackID >= ManosabaMenuID.Margo)
                    BackID = ManosabaMenuID.Ema;
                else
                    BackID += 1;
            }
        }
        public override void PostUpdate()
        {
            Rectangle = Hitbox;
            Position = Center;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ArrowTexture, Position, null, Color.White * Opacity, 0, ArrowTexture.Size() / 2, Scale2 * 0.5f, 0, 0);
        }
    }
}

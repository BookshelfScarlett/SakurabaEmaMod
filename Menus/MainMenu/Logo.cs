using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Menus.Class;
using SakurabaEmaMod.Menus.Managemments;
using SakurabaEmaMod.Menus.PVs;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;

namespace SakurabaEmaMod.Menus.MainMenu
{
    public class Logo : ManosabaButtonClass
    {
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Main_Title.Texture.Value;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Main_Title_CN.Texture.Value;
        public override Vector2 Center => new Vector2(Main.screenWidth/ 2 +500f, 200f);
        public override bool ShouldDrawText => false;
        public override Rectangle Hitbox => Utils.CenteredRectangle(Center, new Vector2(400f, 400f));
        public float HoverScale = 1f;
        public override float ButtonScale => 1f;
        public static bool IsDoneMoonLordFight = false;
        //logo仍然归属于background的一部分
        //因此这里的scaleratios是交给了background管理
        //事实上，因为我们只有一个实例，因此交给他管理是没有问题的
        public static float LogoScaleRatios = 0;
        public override void PostUpdate()
        {
            float scaleRatios = Clamp(Lerp(1.05f, 1f, ManosabaBackground.TheScaleRatios), 1f, 1.05f);
            if (scaleRatios == 1f)
            {
                if (LogoScaleRatios > 0.98f)
                    LogoScaleRatios = 1f;
                else
                    LogoScaleRatios = Clamp(Lerp(LogoScaleRatios, 1f, 0.1f), 0f, 1f);
            }
            //Opactiy在这里不起任何作用，仅作为一个外部可以调用的数据
            //这里Opcatiy主要用于控制在logo完全出现之前，背景的切换禁用
            LogoScaleRatios = Clamp(LogoScaleRatios, 0, 1);
            Opacity = LogoScaleRatios;
            Rectangle = Hitbox;
        }
        public override void MouseHover(bool isHover)
        {
            //如果玩家没首次干掉月总，不要执行任何操作
            //同样如果logo没有完全出现，不要执行操作
            if (LogoScaleRatios < 1)
                return;
            if (!IsDoneMoonLordFight)
                return;
            if(isHover)
            {
                HoverScale = Lerp(HoverScale, 1.2f, 0.1f);
            }
            else
            {
                HoverScale = Lerp(HoverScale, 1f, 0.1f);
            }
        }
        public override void OnMouseLeftRelease()
        {
            //如果玩家没首次干掉月总，不要执行任何操作
            //同样如果logo没有完全出现，不要执行操作
            if (LogoScaleRatios < 1)
                return;
            if (!IsDoneMoonLordFight)
                return;
            if (!BloomVideo.PlayBloom)
                BloomVideo.PlayBloom = true;
            SoundEngine.PlaySound(ManosabaSounds.Menu_GeneralChoice);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Logo接入管理。
            Vector2 logoPos = Center;
            float colorRatios = Clamp(LogoScaleRatios, 0f, 1f);
            //logo也别忘了飞走啊
            //byd写的什么玩意
            bool isCN = Language.ActiveCulture.Name.Equals("zh-Hans");
            Vector2 offset = Vector2.Lerp(Vector2.Zero, Vector2.UnitY * -700f, ManosabaMenuUpdate.ButtonsHoverOut);
            Texture2D chooseTex = isCN ? ChosenTexture : UnChosenTexture;
            spriteBatch.Draw(chooseTex, logoPos + offset, null, Color.White * colorRatios, 0, chooseTex.Size() / 2, 0.52f * ButtonScale * HoverScale, 0, 0);
        }
    }
}

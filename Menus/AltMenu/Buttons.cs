using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.Class;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace SakurabaEmaMod.Menus.AltMenu
{
    public abstract class AltMenuClass : ManosabaButtonClass
    {
        public override Rectangle Hitbox => Utils.CenteredRectangle(Position, new Vector2(600, 75));
        public sealed override bool ShouldDrawText => true;
        public override bool UseOpacity => true;
        public sealed override Texture2D ChosenTexture => ManosabaMenuAssets.Alt_ButtonChosen.Texture.Value;
        public sealed override Texture2D UnChosenTexture => ManosabaMenuAssets.Alt_ButtonUnChosen.Texture.Value;
        public override int UIDepth => 2;
        public override bool PreSetDepth() => false;
        /// <summary>
        /// 与下一个按钮之间的gap。这里取的中心点之间的gap
        /// </summary>
        public virtual float ButtonGap { get; }
        /// <summary>
        /// 按钮从上到下的先后顺序
        /// </summary>
        public virtual int ButtonNumber { get; }
        /// <summary>
        /// 第一个按钮与最上方屏幕的距离
        /// </summary>
        public virtual float FirstButtonOffset { get; }
        public override Vector2 Center => new Vector2(Main.screenWidth / 2f, FirstButtonOffset + (ButtonNumber - 1) * ButtonGap);
        public override void OnMouseLeftRelease()
        {
            //过度没完全完成的前提下，下面的更新都是不会跑的
            //这里是为了防止可能的意外
            if (ManosabaMenuUpdate.BanSwitchMenu)
                return;
            //textValue需要更新。这里用于转入泰拉界面之后的角标
            ManosabaMenuDraw.DrawTextValue = TextValue;
            ManosabaMenuLayer.OverlayBlackOpacity = 0;
            ManosabaMenuUpdate.GeneralFadingRatios = 0;
            ExOnMouseLeftRelease();
        }
        public virtual void ExOnMouseLeftRelease() { }
    }
    public class SinglePlayer : AltMenuClass
    {
        public override float FirstButtonOffset => 250f;
        public override int ButtonNumber => 1;
        public override float ButtonGap => 150f;
        public override string TextValue => Mod.GetLocalizationKey("Menu.SinglePlayer").ToLangValue();
        public override float ButtonScale => 0.5f;
        public override int TargetMenuID => MenuID.CharacterSelect;
        public override void ExOnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_LoadGame with { volume = 0.7f });
            ManosabaMenuMethods.ChangeMenu(TargetMenuID);
        }
    }
    public class Multiplayer : AltMenuClass
    {
        public override float FirstButtonOffset => 250f;
        public override int ButtonNumber => 2;
        public override float ButtonGap => 150f;
        public override string TextValue => Mod.GetLocalizationKey("Menu.Multiplayer").ToLangValue();
        public override float ButtonScale => 0.5f;
        public override int TargetMenuID => MenuID.Multiplayer;
        public override void ExOnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_LoadGame with { volume = 0.7f });
            ManosabaMenuMethods.ChangeMenu(TargetMenuID);
        }

    }
    public class Workshop : AltMenuClass
    {
        public override float FirstButtonOffset => 250f;
        public override int ButtonNumber => 3;
        public override float ButtonGap => 150f;
        public override string TextValue => Mod.GetLocalizationKey("Menu.Workshop").ToLangValue();
        public override float ButtonScale => 0.5f;
        public override int TargetMenuID => MenuID.FancyUI;
        public override void ExOnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_GeneralChoice);
            ManosabaMenuMethods.OpenWorkshop();
        }
    }
    public class Credit : AltMenuClass
    {
        public override float FirstButtonOffset => 350f;
        public override float ButtonGap => 150f;
        public override int ButtonNumber => 2;
        public override int TargetMenuID => MenuID.CreditsRoll;
        public override string TextValue => Mod.GetLocalizationKey("Menu.Credit").ToLangValue();
        public override float ButtonScale => 0.5f;
        public override void OnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_GeneralChoice);
            ManosabaMenuMethods.ChangeMenu(TargetMenuID);
            ManosabaMenuUpdate.OnChangeToTargetMenuID.Add(delegate
            {
                SkyManager.Instance.Activate("CreditsRoll");
            });
        }
    }
    public class Achievement : AltMenuClass
    {
        public override float FirstButtonOffset => 350f;
        public override float ButtonGap => 150f;
        public override string TextValue => Mod.GetLocalizationKey("Menu.Achievement").ToLangValue();
        public override float ButtonScale => 0.5f;
        public override int ButtonNumber => 1;
        public override int TargetMenuID => MenuID.FancyUI;
        public override void OnMouseLeftRelease()
        {
            SoundEngine.PlaySound(ManosabaSounds.Menu_GeneralChoice);
            ManosabaMenuDraw.DrawTextValue = TextValue;
            ManosabaMenuMethods.OpenAchievements();
        }
    }
    public abstract class CancelButtonClass : ManosabaButtonClass
    {
        public override bool UseOpacity => true;
        public override Texture2D UnChosenTexture => ManosabaMenuAssets.Alt_ExitUnChosen.Texture.Value;
        public override float ButtonScale => 0.5f;
        public override Texture2D ChosenTexture => ManosabaMenuAssets.Alt_ExitChosen.Texture.Value;
        public override bool ShouldDrawText => false;
        public override Vector2 Center => new Vector2(Main.screenWidth - 50f, 50f);
        public override int UIDepth => 2;
        public override int TargetMenuID => MenuID.None;
        public override bool PreSetDepth() => false;
    
    }
    public class CancelLoad : CancelButtonClass
    {
        /// <summary>
        /// 仍然不应该阻止游戏玩家按下右键取消二级ui。
        /// 但是留这个按钮用于提升ui体验。
        /// </summary>
        public override void OnMouseLeftRelease()
        {
            if (LoadHud.IsEnable)
                LoadHud.IsFading = true;
            SoundEngine.PlaySound(ManosabaSounds.Menu_Cancel);
        }
    }
    public class CancelGallery : CancelButtonClass 
    {
        public override void OnMouseLeftRelease()
        {
            if (GalleryHud.IsEnable)
                GalleryHud.IsFading = true;
            SoundEngine.PlaySound(ManosabaSounds.Menu_Cancel);
        }

    }
}

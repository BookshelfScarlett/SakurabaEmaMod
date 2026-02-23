using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.MainMenu;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Menus.Class
{
    public abstract class ManosabaButtonClass : ManosabaHud 
    {
        /// <summary>
        /// 我草我因为自己写的超级史山代码导致我不得不新建一个是否用默认Opactiy
        /// 我是傻逼吧
        /// 总之，如果设定为True，则对于“不绘制文本“的按钮，会使用基类提供的Opacity
        /// </summary>
        public virtual bool UseOpacity => false;
        /// <summary>
        /// 魔裁的贴图需要准备两份。一份为选择的，一份为未选择的
        /// 这份为选择的那个
        /// </summary>
        public virtual Texture2D ChosenTexture { get; }
        /// <summary>
        /// 魔裁的贴图需要准备两份。一份为选择的，一份为未选择的
        /// 这份为选择的那个
        /// </summary>
        public virtual Texture2D UnChosenTexture { get; } 
        /// <summary>
        /// 贴图位置的偏移
        /// 因为使用的贴图由于裁剪问题并不一定是完全大小一致的
        /// 输入这个的话，用于在校准“悬停”时，贴图可能产生的位移
        /// </summary>
        public virtual Vector2 TexturePosOffset => Vector2.Zero;
        /// <summary>
        /// 绘制使用。你不应该直接诶修改这个值，也一般不用
        /// </summary>
        public Vector2 ActualTexturePosOffset = Vector2.Zero;
        /// <summary>
        /// 文本颜色，无需复写，因为会在MouseHover那随时进行更新
        /// 魔裁如此
        /// </summary>
        public Color TextColor = Color.White;
        /// <summary>
        /// 是否绘制文本
        /// 用于部分艺术性按钮，即适用的按钮贴图上本来就有艺术性字体时
        /// </summary>
        public virtual bool ShouldDrawText => true;
        /// <summary>
        /// 文本内容
        /// </summary>
        public virtual string TextValue { get; }
        /// <summary>
        /// 按钮绘制的大小
        /// 只作用于draw
        /// </summary>
        public virtual float ButtonScale => 1f;
        /// <summary>
        /// 按钮的中心点位置
        /// </summary>
        public virtual Vector2 Center => Vector2.Zero;
        public virtual int TargetMenuID => MenuID.CharacterSelect;
        public virtual Rectangle Hitbox => Utils.CenteredRectangle(Position, new Vector2(180, 60));
        /// <summary>
        /// 位于PostUpdate钩子内最后执行
        /// </summary>
        public virtual void FinalPostUpdate() { }
        public float EdgeOpacity;
        public float XEdgeScale;
        public bool MouseIsHovering = false;
        public override void SetDefaults()
        {
            Position = Center;
            Rectangle = Hitbox;
            Opacity = 1f;
        }
        public override void StartHover()
        {
            //SakurabaEmaMod.Instance.Logger.Error($"{GetType().Name} of Pos: {Center}, CurrentMenu : {Main.menuMode}");
        }
        public override void MouseHover(bool isHover)
        {
            //鼠标悬停时候的行为
            //将两份不同的贴图直接替换。
            MouseIsHovering = isHover;
        }
        public override void OnLeftClick()
        {
        }
        public override void MouseLeft()
        {
        }
        public override void OnMouseLeftRelease()
        {
            ManosabaMenuMethods.ChangeMenu(TargetMenuID);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ShouldDrawText)
            {
                DrawWithTextValue(spriteBatch);
            }
            else
            {
                Texture2D useTex = MouseIsHovering ? ChosenTexture : UnChosenTexture;
                Vector2 posOffset = MouseIsHovering ? TexturePosOffset : Vector2.Zero;
                //实际绘制
                float colorRatios = Clamp(ManosabaBackground.LogoScaleRatios, 0f, 1f);
                Color useColor = UseOpacity ? Color.White * Opacity : Color.White * colorRatios;
                spriteBatch.Draw(useTex, Position + posOffset, null, useColor, Rotation, useTex.Size() / 2, ButtonScale, 0, 0);
            }
            
        }
        public virtual void DrawWithTextValue(SpriteBatch spriteBatch)
        {
            Texture2D useTex = MouseIsHovering ? ChosenTexture : UnChosenTexture;
            Vector2 posOffset = MouseIsHovering ? TexturePosOffset : Vector2.Zero;
            Color textColor = MouseIsHovering ? Color.White : Color.Black;
            //实际绘制
            spriteBatch.Draw(useTex, Position + posOffset, null, Color.White * Opacity, 0, useTex.Size() / 2, ButtonScale, 0, 0);
            DynamicSpriteFont dynamicSpriteFont = ManosabaFonts.等线.Value;
            Vector2 scale = new(0.92f);
            Vector2 Size = ChatManager.GetStringSize(dynamicSpriteFont, TextValue, scale);
            Vector2 ori = Size / 2;
            //绘制的位置一定程度上需要偏移
            //考虑到这里只有一个横条按钮需要用到这个文本。直接硬编码
            Vector2 textPos = Center + Vector2.UnitY * 10f + Vector2.UnitX * -6f;
            ChatManager.DrawColorCodedString(spriteBatch, dynamicSpriteFont, TextValue, textPos, textColor * Opacity, 0, ori, scale);
        }
        public override void PostUpdate()
        {
            //我没更新？
            Position = Center;
            Rectangle = Hitbox;
            FinalPostUpdate();
        }

    }
}

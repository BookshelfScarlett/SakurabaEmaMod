using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Globals.Enums;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core.Hud
{
    public class CutsceneHud : ModType
    {
        /// <summary>
        /// 转场的位置
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// 转场大小，浮点数
        /// </summary>
        public float Scale;
        /// <summary>
        /// 转场的透明度
        /// </summary>
        public float Opacity;
        /// <summary>
        /// 这个视频的“碰撞箱"
        /// </summary>
        public Rectangle Hitbox;
        /// <summary>
        /// 是否允许这个专场“可碰撞"
        ///默认为否
        /// </summary>
        public bool ShouldCollision;
        /// <summary>
        /// 专场的类型
        /// 目前给Bloom提供了一个专属的Enum
        /// </summary>
        public CutsceneType SetCutsceneType;
        /// <summary>
        /// 专场开始的标记
        /// </summary>
        public bool IsStart;
        /// <summary>
        /// 专场结束的标记
        /// </summary>
        public bool IsEnd;
        /// <summary>
        /// 专场是否正在进行
        /// </summary>
        public bool IsNowPlaying;
        /// <summary>
        /// 是否允许释放当前转场（确切来说，把当前的转场重置为“Null”
        /// </summary>
        public bool ShouldEndCutscene = false;
        /// <summary>
        /// 转场进程
        /// </summary>
        public float Timer;
        public float LifeTime;
        public float LifeTimeRatio => Clamp(Timer / LifeTime, 0f, 1f);
        public bool IsColliding;
        /// <summary>
        /// 这个专场是否直接是个视频
        /// 目前暂时用不到，但是预留了
        /// 默认设置为True且不可改
        /// </summary>
        public bool IsVideo => true;

        protected sealed override void Register()
        {
            SetDefaults();
        }
        /// <summary>
        /// 注册默认数据
        /// </summary>
        public virtual void SetDefaults()
        {
            Position = Vector2.Zero;
            Scale = 1f;
            Opacity = 1f;
            Hitbox = new Rectangle();
            ShouldCollision = false;
            SetCutsceneType = CutsceneType.None;
            IsNowPlaying = false;
            IsStart = false;
            IsEnd = false;
        }
        /// <summary>
        /// 转场的更新钩子
        /// 如果这个专场确认是个视频，我不建议你复写这个钩子
        /// </summary>
        public void Update()
        {
            IsColliding = Colliding(Hitbox, ManosabaModSystem.MouseRectangle) && ShouldCollision;
            if (IsColliding)
                OnColiision();
            if (IsVideo)
                UpdateVideo();
            else
                UpdateMisc();
            PostUpdate();
        }
        /// <summary>
        /// 在“发生”交互碰撞时，执行的行为
        /// </summary>
        public virtual void OnColiision() { }

        /// <summary>
        /// 判断指针与视频hitbox是否碰撞，且在发生碰撞时执行什么行为
        /// 建议添加特定条件再返回需要得值。不然会默认返回真
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="mouseHitbox"></param>
        public virtual bool Colliding(Rectangle hitbox, Rectangle mouseHitbox)
        {
            return hitbox.Contains(Main.MouseScreen.ToPoint());
        }
        /// <summary>
        /// 专场开始时，执行的任务
        /// </summary>
        public virtual void OnStart() { IsStart = false; }
        /// <summary>
        /// 专场结束时，执行的任务
        /// </summary>
        public virtual void OnEnd() { IsEnd = false; }
        /// <summary>
        /// 在Update方法内，最后更新
        /// </summary>
        public virtual void PostUpdate()
        {
        }

        /// <summary>
        /// 非视频更新钩子
        /// IsVideo为否时启用
        /// </summary>

        public virtual void UpdateMisc()
        {
        }

        /// <summary>
        /// 视频更新钩子
        /// 在IsVideo为否时不启用
        /// </summary>
        public virtual void UpdateVideo()
        {
        }
        /// <summary>
        /// 记得传画笔。
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb) { }
    }
}

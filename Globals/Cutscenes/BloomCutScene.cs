using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ReLogic.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Menus.MainMenu;
using SakurabaEmaMod.Menus.Managemments;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SakurabaEmaMod.Globals.Cutscenes
{
    public class BloomCutScene : CutsceneHud
    {
        public static bool PlayBloom;
        public static bool IsDone;
        public float FlyInTimer = 0;
        public float HandleTime = 0;
        public float TextTimer = 0;
        public float InnerTimer = 0;
        public bool FadingText;
        public override void SetDefaults()
        {
            Position = ScreenCenter;
            //后续是可能制作其他专场的
            SetCutsceneType = Enums.CutsceneType.Bloom;
            Scale = 1f;
            Opacity = 1f;
        }
        public override void PostUpdate()
        {
        }
        public override void UpdateVideo()
        {
            HandleFlyIn();
            HandleUpdateText();
            HanldeVideoPlay();
        }


        private void HanldeVideoPlay()
        {
            /*
            这里不能新建一个视频播放的实例。
            主要原因是，这里必须得想办法让VideoPlayer播放的视频能够在退出世界时正常关闭，因为这里的CutScene是在游戏内播报的
            但是，VideoPlayer不会在你手动退出世界的时候释放资源
            尽管ModSystem内有一个OnUnloadWorld的钩子，但是你即使在这里静态Video然后尝试在OnUnloadWorld释放的话
            他仍然会提示一个报错。因为VideoPlayer本身不能在任何“子线程”里面运行，而是必须得在主线程运行
            因此这里必须得直接调用已经进行过Load的视频实例，即Menu里面的视频实例
            */
            ManosabaMenu.BloomVideo.Volume = Main.musicVolume;
            Main.LocalPlayer.ManosabaMod().IsPlayingBloom = true;
            //这里会直接做掉游戏当前的音乐
            //直到需要的时候才会释放
            if (IsDone && ManosabaMenu.BloomVideo.State == MediaState.Stopped)
                EndHandler();
            if (IsDone && ManosabaMenu.BloomVideo.State != MediaState.Stopped)
            {
                //右键直接执行这个指令，让manager知道你需要退出了，同上
                if (Main.mouseRight)
                {
                    EndHandler();
                }
            }
            void EndHandler()
            {
                ShouldEndCutscene = true;
                FlyInTimer = 0;
                IsDone = false;
            }

        }

        private void HandleFlyIn()
        {
            FlyInTimer = Lerp(FlyInTimer, 1f, 0.1f);
            if (FlyInTimer > 0.98f)
                FlyInTimer = 1;
        }

        /// <summary>
        /// 提示文本的渐入和渐出
        /// 本质史山代码。
        /// 但是你放过我得了。
        /// </summary>
        private void HandleUpdateText()
        {
            InnerTimer++;
            if (FadingText)
            {
                if (InnerTimer > GetSeconds(5))
                    TextTimer = Lerp(TextTimer, 0f, 0.1f);
                if (TextTimer < 0.02f)
                    TextTimer = 0;
            }
            else
            {
                TextTimer = Lerp(TextTimer, 1f, 0.1f);
                if (TextTimer > 0.98f)
                {
                    TextTimer = 1;
                    FadingText = true;
                }
            }
        }

        public override void OnColiision()
        {

        }
        public override void OnStart()
        {
            //卡住这里一点时间然后我们再开始
            //开始播放这个玩意。
            HandleTime = Lerp(HandleTime, 1f, 0.01f);
            if (HandleTime < 0.98f)
                return;
            if (ManosabaMenu.BloomVideo.State == MediaState.Stopped)
            {
                HandleTime = 1f;
                //代办：这里还有一个疑似的Loop问题
                ManosabaMenu.BloomVideo.IsLooped = false;
                //这边播放有一个音画不同步的问题
                //暂时不知道咋解决。
                //解决了，但是实际运行仍然会
                //期盼一下玩家电脑问题得了。
                ManosabaMenu.BloomVideo.Play(ManosabaVideo.BloomPV.Value);
                FadingText = false;
                IsDone = true;
                IsNowPlaying = true;
                IsStart = false;
                
            }
        }
        /// <summary>
        /// 下面的这些reset应该是不必要的代码
        /// 但我本人对基础的代码知识并不是很过关，也没有进行多次测试，先放在这
        /// </summary>
        public void ResetData()
        {
            FlyInTimer = 0;
            HandleTime = 0;
            InnerTimer = 0;
            TextTimer = 0;
        }
        public override void OnEnd()
        {
            ManosabaMenu.BloomVideo.Stop();
            HandleTime = Lerp(HandleTime, 0f, 0.1f);
            if (HandleTime < 0.01f)
            {
                //重置一下这个状态
                ShouldEndCutscene = false;
                FadingText = false;
                ResetData();
                if (!Logo.IsDoneMoonLordFight)
                {
                    //end成功的时候使用Main.newText发送给本地玩家提示可以用主界面logo来播放bloom
                    string textValue = Mod.GetLocalizationKey("Menu.LogoCanPlay").ToLangValue();
                    Main.NewText(textValue);
                }
                //记得设置logo的情况，这是个静态数据，所以没问题
                Logo.IsDoneMoonLordFight = true;
                Main.LocalPlayer.ManosabaMod().IsPlayingBloom = false;
                Main.LocalPlayer.ManosabaMod().IsDoneFinalBossFight = true;
                base.OnEnd();
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            DrawBlackScreen(sb);
            if (!IsNowPlaying)
                return;

            Texture2D videoTex = ManosabaMenu.BloomVideo.GetTexture();
            if (videoTex != null)
            {
                float scale = Lerp(0f, 1, FlyInTimer);
                sb.Draw(videoTex, ScreenCenter, null, Color.White * scale, 0, videoTex.Size() / 2, Scale, 0, 0);
                DrawExitTextvalue(sb);
            }
        }
        public void DrawBlackScreen(SpriteBatch sb)
        {
            //入场也好还是退场也罢这里都会提供一个黑屏渐变
            //刚好pv都是黑屏出入，可以偷下懒了
            sb.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * HandleTime);

        }
        public void DrawExitTextvalue(SpriteBatch sb)
        {
            string TextValue = Mod.GetLocalizationKey("Menu.InGameContext").ToLangValue();
            DynamicSpriteFont dynamicSpriteFont = ManosabaFonts.等线.Value;
            Vector2 scale2 = new(1.0f);
            Vector2 Size = ChatManager.GetStringSize(dynamicSpriteFont, TextValue, scale2);
            Vector2 ori = Size / 2;
            //绘制的位置一定程度上需要偏移
            //考虑到这里只有一个横条按钮需要用到这个文本。直接硬编码
            Vector2 textPos = ScreenCenter + Vector2.UnitY * 400f;
            for (int i = 0; i < 8; i++)
                ChatManager.DrawColorCodedString(sb, dynamicSpriteFont, TextValue, textPos + ToRadians(60f * i).ToRotationVector2() * 2f, Color.Black * TextTimer, 0, ori, scale2);
            ChatManager.DrawColorCodedString(sb, dynamicSpriteFont, TextValue, textPos, Color.White * TextTimer, 0, ori, scale2);

        }
    }
}

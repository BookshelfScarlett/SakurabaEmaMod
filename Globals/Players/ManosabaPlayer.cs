using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Items.Vanity;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SakurabaEmaMod.Globals.Players
{
    /// <summary>
    /// 这个Player类用于统一管理之后可能到来的……一系列角色
    /// 目前因为只有单独的一套艾玛，所以这里实际上本质什么都不会做。
    /// </summary>
    public partial class ManosabaPlayer : ModPlayer
    {
        #region 时装佩戴的Boolen
        //扔给了统一的管理。
        public float ParticleTimer = 0;
        public short ManosabaGirl = ManosabaGirlID.None;
        public bool NoahCryDeath = false;
        public bool EmaKiangSound = false;
        public bool NoaButterflyDeath = false;
        public bool sakurabaEmaVanity = false;
        public bool IsDoneFinalBossFight = false;
        public bool IsPlayingBloom = false;
        #endregion
        #region 其余内容
        public bool IsGiveAnyVanityItem = false;
        #endregion

        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(ManosabaGirl), ManosabaGirl);
            tag.Add(nameof(IsGiveAnyVanityItem), IsGiveAnyVanityItem);
            tag.Add(nameof(NoaButterflyDeath), NoaButterflyDeath);

            tag.Add(nameof(EmaKiangSound), EmaKiangSound);
            tag.Add(nameof(NoahCryDeath), NoahCryDeath);

        }
        public override void LoadData(TagCompound tag)
        {
            ManosabaGirl = tag.GetShort(nameof(ManosabaGirl));
            NoaButterflyDeath = tag.GetBool(nameof(NoaButterflyDeath));
            IsGiveAnyVanityItem = tag.GetBool(nameof(IsGiveAnyVanityItem));

        }
        public override void OnEnterWorld()
        {
            if (Main.myPlayer != Player.whoAmI)
                return;
            if (IsGiveAnyVanityItem)
                return;
            string name = Player.name.ToLower();
            if(name.Contains("sakuraba") || name.Contains("ema") || name.Contains("樱羽艾玛") || name.Contains("艾玛"))
            {
                IsGiveAnyVanityItem = true;
                Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<SakurabaEmma>());
            }
            if (name.Contains("natsume") || name.Contains("anan") || name.Contains("夏目安安") || name.Contains("安安"))
            {
                IsGiveAnyVanityItem = true;
                Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<NatsumeAnanItem>());
            }
            if (name.Contains("nikaidou") || name.Contains("hiro") || name.Contains("二阶堂希罗") || name.Contains("希罗"))
            {
                IsGiveAnyVanityItem = true;
                Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<NikaidouHiroItem>());
            }
            if (name.Contains("jougasaki") || name.Contains("noa") || name.Contains("城崎诺亚") || name.Contains("诺亚"))
            {
                IsGiveAnyVanityItem= true;
                Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<JougasakiNoahItem>());
            }
        }
        public override void ResetEffects()
        {
            ManosabaGirl = ManosabaGirlID.None;
            NoaButterflyDeath = false;
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            //是的孩子们在播放ed的时候玩家会直接无敌
            //我才不管这样会不会有人逃课呢我都把整个屏幕盖住了
            if (IsPlayingBloom)
                return true;
            return false;
        }
        public override void FrameEffects()
        {
            if (ManosabaGirl != ManosabaGirlID.None)
            {
                UpdateVanityOnNeed();
                DrawParticleOnNeed(); 
            }
        }
        public override void PostUpdate()
        {
            bool isInInventory = Main.mouseMiddle && Main.playerInventory;
            if (!isInInventory)
                return;
            if (!Main.mouseMiddleRelease)
                return;
            //我的天哪还有i诺TV
            if (Main.HoverItem.type == ItemType<JougasakiNoahItem>())
            {
                string Path = "SakurabaEmaMod/Assets/Sounds/";
                SoundStyle playSound = !NoahCryDeath ? new SoundStyle($"{Path}{nameof(Charactor.JougasakiNoah)}Sounds/Hit6") : new SoundStyle($"{Path}{nameof(Charactor.JougasakiNoah)}Sounds/Hit1");
                SoundEngine.PlaySound(playSound, Player.Center);
                NoahCryDeath = !NoahCryDeath;
            }
            //我的天哪还有i玛TV
            if (Main.HoverItem.type == ItemType<SakurabaEmma>())
            {
                SoundStyle playSound = !EmaKiangSound ? ManosabaSounds.Ema_Kiang : ManosabaSounds.Ema_HitSound;
                SoundEngine.PlaySound(playSound, Player.Center);
                EmaKiangSound = !EmaKiangSound;
            }
        }
        public override void UpdateVisibleVanityAccessories()
        {
            bool isPausingGame = Main.gamePaused || Main.autoPause;
            if (ManosabaGirl != ManosabaGirlID.None && isPausingGame)
                UpdateVanityOnNeed();
        }
        public void UpdateVanityOnNeed()
        {
            if (ManosabaGirl == ManosabaGirlID.None)
                return;
            string name = $"{GetName}Item";
            //艾玛由于最开始的类名问题，而且因为已经实装很久了，这里没法像上面一样接入到统一管理内
            //因此这里是手动进行的特判
            if (ManosabaGirl == ManosabaGirlID.SakurabaEma)
                name = nameof(SakurabaEmma);
            //一些特殊情况。如安安有一个额外的头发……
            //怎么都是特殊情况。
            if (ManosabaGirl == ManosabaGirlID.NatsumeAnan || ManosabaGirl == ManosabaGirlID.NikaidouHiro || ManosabaGirl == ManosabaGirlID.JougasakiNoah)
                Player.back = EquipLoader.GetEquipSlot(Mod, name, EquipType.Back);
            Player.legs = EquipLoader.GetEquipSlot(Mod, name, EquipType.Legs);
            Player.body = EquipLoader.GetEquipSlot(Mod, name, EquipType.Body);
            Player.head = EquipLoader.GetEquipSlot(Mod, name, EquipType.Head);
        }

        /// <summary>
        /// string有个问题是过度占用空间，而且高频调用的情况下有可能有一定的性能问题
        /// 这里改为了enum（本质short）与玩家类的short，然后用switch的形式在需要的时候使用nameof返回
        /// </summary>
        private string GetName
        {
            get
            {
                return ManosabaGirl switch
                {
                    ManosabaGirlID.NikaidouHiro => nameof(Charactor.NikaidouHiro),
                    ManosabaGirlID.NatsumeAnan => nameof(Charactor.NatsumeAnan),
                    ManosabaGirlID.JougasakiNoah => nameof(Charactor.JougasakiNoah),
                    ManosabaGirlID.TachibanaSherry => nameof(Charactor.TachibanaSherry),
                    ManosabaGirlID.ToonoHanna => nameof(Charactor.ToonoHanna),
                    _ => nameof(Charactor.None),
                };
            }
        }
    }
}

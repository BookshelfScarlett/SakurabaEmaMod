using Microsoft.Xna.Framework;
using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Items.Vanity;
using System;
using Terraria;
using Terraria.ID;
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
        public bool NoaButterflyDeath = false;
        public bool sakurabaEmaVanity = false;
        #endregion
        #region 其余内容
        public bool IsGiveAnyVanityItem = false;
        #endregion

        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(ManosabaGirl), ManosabaGirl);
            tag.Add(nameof(NoaButterflyDeath), NoaButterflyDeath);
            tag.Add(nameof(IsGiveAnyVanityItem), IsGiveAnyVanityItem);

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
            if (name.Contains("Natsume") || name.Contains("Anan") || name.Contains("夏目安安") || name.Contains("安安"))
            {
                IsGiveAnyVanityItem = true;
                Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<NatsumeAnanItem>());
            }
        }
        public override void ResetEffects()
        {
            ManosabaGirl = ManosabaGirlID.None;
            NoaButterflyDeath = false;
        }
        public override void FrameEffects()
        {
            //这里只能通过遍历所有玩家原版盔甲栏的方式来寻找需要的时装物品。
            //因为这里需要实现的效果是，让人物存档页面也能绘制需要的时装
            //如果玩家佩戴了其他的饰品栏，那么……嗯，随便吧反正
            bool equip = false;
            if (Main.gameMenu)
                GetArmorSlotItem(ref equip);
            if (ManosabaGirl != ManosabaGirlID.None || equip)
            {
                UpdateVanityOnNeed();
                DrawParticleOnNeed(); 
            }
        }
        public void GetArmorSlotItem(ref bool equip)
        {
            foreach (Item item in Player.armor)
            {
                if (item.type == GetItemType)
                {
                    equip = true;
                    break;
                }
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
            //除了艾玛的时装以外都统一管理
            //不过话又说回来，谁能想到这个mod会拓展成魔裁mod呢？一开始只是想把艾玛做进去罢了
            string name = $"{GetName}Item";
            //一些特殊情况。如安安有一个额外的头发……
            if (ManosabaGirl == ManosabaGirlID.NatsumeAnan)
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
                    ManosabaGirlID.SakurabaEma => nameof(Charactor.SakurabaEma),
                    ManosabaGirlID.NikaidouHiro => nameof(Charactor.NikaidouHiro),
                    ManosabaGirlID.NatsumeAnan => nameof(Charactor.NatsumeAnan),
                    ManosabaGirlID.JougasakiNoa => nameof(Charactor.JougasakiNoa),
                    ManosabaGirlID.TachibanaSherry => nameof(Charactor.TachibanaSherry),
                    ManosabaGirlID.ToonoHanna => nameof(Charactor.ToonoHanna),
                    _ => nameof(Charactor.None),
                };
            }
        }
        private int GetItemType
        {
            get
            {
                return ManosabaGirl switch
                {
                    ManosabaGirlID.SakurabaEma => ItemType<SakurabaEmma>(),
                    ManosabaGirlID.NatsumeAnan => ItemType<NatsumeAnanItem>(),
                    _ => -1,
                };

            }
        }
    }
}

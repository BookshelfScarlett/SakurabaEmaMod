using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

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
        public Charactor CurrentCharator = Charactor.None;
        public bool sakurabaEmaVanity = false;
        #endregion
        #region 其余内容

        #endregion
        
        //public override void SaveData(TagCompound tag)
        //{
        //    tag.Add(nameof(CurrentCharator), CurrentCharator);
        //}
        //public override void LoadData(TagCompound tag)
        //{
        //    CurrentCharator = tag.Get<Charactor>(nameof(CurrentCharator));
        //}
        public override void ResetEffects()
        {
            CurrentCharator = Charactor.None;
        }
        public override void FrameEffects()
        {
            //这里只能通过遍历所有玩家原版盔甲栏的方式来寻找需要的时装物品。
            //因为这里需要实现的效果是，让人物存档页面也能绘制需要的时装
            //如果玩家佩戴了其他的饰品栏，那么……嗯，随便吧反正
            bool equip = false;
            if (Main.gameMenu)
                GetArmorSlotItem(ref equip);
            if (CurrentCharator != Charactor.None || equip)
                UpdateVanityOnNeed();
        }
        public void GetArmorSlotItem(ref bool equip)
        {
            foreach (Item item in Player.armor)
            {
                if (item.type == ItemType<SakurabaEmma>())
                {
                    equip = true;
                    break;
                }
            }
        }
        public override void UpdateVisibleVanityAccessories()
        {
            bool isPausingGame = Main.gamePaused || Main.autoPause;
            if (CurrentCharator != Charactor.None  && isPausingGame)
                UpdateVanityOnNeed();
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
        public void CustomSoundOnHurt(Player.HurtInfo hurtInfo)
        {
            if (!DisableOrignalSound)
                return;
            hurtInfo.SoundDisabled = true;
            //代办：其余角色的承伤音效

        }
        public void ModifyCustomSound(ref  Player.HurtModifiers modifiers)
        {
            if (!DisableOrignalSound)
                return;
            modifiers.DisableSound();
        }
        public bool DisableOrignalSound
        {
            get
            {
                return CurrentCharator != Charactor.None;
            }
        }
        public void UpdateVanityOnNeed()
        {
            if (CurrentCharator == Charactor.None)
                return;
            //除了艾玛的时装以外都统一管理
            //不过话又说回来，谁能想到这个mod会拓展成魔裁mod呢？一开始只是想把艾玛做进去罢了
            Player.legs = EquipLoader.GetEquipSlot(Mod, nameof(CurrentCharator), EquipType.Legs);
            Player.body = EquipLoader.GetEquipSlot(Mod, nameof(CurrentCharator), EquipType.Body);
            Player.head = EquipLoader.GetEquipSlot(Mod, nameof(CurrentCharator), EquipType.Head);

        }

    }
}

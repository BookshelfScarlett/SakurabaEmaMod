using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.NetCodes;
using SakurabaEmaMod.Globals.Methods;
using SakurabaEmaMod.Particles;
using SakurabaEmaMod.Rarity.RarityShiny;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SakurabaEmaMod.Items.Vanity
{
    public class SakurabaEmaPlayer : ModPlayer
    {
        public bool vanityEquipped = false;
        public bool JustKiang = false;
        public bool isGivedItem = false;
        public float Timer = 0;
        public override void LoadData(TagCompound tag)
        {
            //这些信息在退出世界的时候都不会保存
            vanityEquipped = tag.GetBool(nameof(vanityEquipped));
            JustKiang = tag.GetBool(nameof(JustKiang));
            isGivedItem = tag.GetBool(nameof(isGivedItem));
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(vanityEquipped), vanityEquipped);
            tag.Add(nameof(JustKiang), JustKiang);
            tag.Add(nameof(isGivedItem), isGivedItem);
        }
        public override void ResetEffects()
        {
            vanityEquipped = false;
        }
        //Timer不要直接设定为0
        public override void UpdateDead() => Timer = 1;
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!ShouldDisable)
            {
                hurtInfo.SoundDisabled = true;
                Disable();
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!ShouldDisable)
            {
                hurtInfo.SoundDisabled = true;
                Disable();
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if(!ShouldDisable)
            {
                info.SoundDisabled = true;
                Disable();
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers) => ModifySound(ref modifiers);
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) => ModifySound(ref modifiers);
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) => ModifySound(ref modifiers);

        //modify这里取消声音就行了，自定义音效在onhit那执行
        private void ModifySound(ref Player.HurtModifiers modifiers)
        {
            if (!ShouldDisable)
                modifiers.DisableSound();

        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (vanityEquipped)
            {
                genDust = false;
                if (!ShouldDisable)
                {
                    SoundStyle sound = JustKiang ? ManosabaSounds.Ema_Kiang : ManosabaSounds.Ema_HitHeavy;
                    SoundEngine.PlaySound(sound, Player.Center);
                }
                //落樱和散发粒子，修改为这些。
                new CrossGlow(Player.Center, Color.Pink, 30, 1f, 0.13f, false).Spawn();
                for (int i = 0; i < 15; i++)
                {
                    Vector2 spawnPos = Player.Center.ToRandCirclePos(36f);
                    new Petal(spawnPos, Vector2.UnitY * Main.rand.NextFloat(1.1f, 1.3f), RandLerpColor(Color.HotPink, Color.LightPink), 120, RandRotTwoPi, 0.8f, Main.rand.NextFloat(0.08f, 0.1f), 0.3f).Spawn();
                    new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(4), 0.2f, RandLerpColor(Color.HotPink, Color.LightPink), 120, 0.22f, RandRotTwoPi).Spawn();

                }
            }
            //当然仍然可以杀死玩家。
            return true;
        }
        public override void OnEnterWorld()
        {
            if (Main.myPlayer == Player.whoAmI && !isGivedItem)
            {
                string name = "SakurabaEma".ToLower();
                string name2 = "樱羽艾玛";
                if (Player.name.ToLower().Equals(name) || Player.name.ToLower().Contains("sakuraba") || Player.name.Contains(name2))
                {
                    Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ItemType<SakurabaEmma>());
                    isGivedItem = true;
                }
            }
        }
        public override void PostUpdate()
        {
            if (Main.mouseMiddle && Main.HoverItem.type == ItemType<SakurabaEmma>() && Main.playerInventory)
            {
                if (!Main.mouseMiddleRelease)
                    return;
                SoundStyle playSound = !JustKiang ? ManosabaSounds.Ema_Kiang : ManosabaSounds.Ema_HitSound;
                SoundEngine.PlaySound(playSound, Player.Center);
                JustKiang = !JustKiang;
            }
        }
        private bool ShouldDisable
        {
            get
            {
                //出于某些原因如果真的有神人玩家选择加载了樱羽艾玛死亡音效mod，则禁用这个物品的所有自定义音效
                //还有这里的中文变量名纯故意的
                bool 草kino = ModLoader.HasMod("Sounds_SakurabaEma");
                if (草kino)
                    return true;
                if (!vanityEquipped)
                    return true;
                return false;
            }
        }
        private bool Disable()
        {
            //如果玩家出于某种原因想听艾玛狗叫的话。
            //kiang
            if (Main.netMode == NetmodeID.Server)
            {
                int soundType;
                if (JustKiang)
                {
                    //是的，这里漂洋过海半天就是为了做包的收发。
                    soundType = 1;
                    SoundPackets.BroadcastSound(Player.Center, soundType);
                }
                else
                {
                    soundType = Main.rand.NextBool().ToInt();
                    SoundPackets.BroadcastSound(Player.Center, soundType);
                }
            }
            else
            {
                if (JustKiang)
                {
                    //是的，这里漂洋过海半天就是为了做包的收发。
                    SoundEngine.PlaySound(ManosabaSounds.Ema_Kiang, Player.Center);
                }
                else
                {
                    SoundStyle hitSound = Main.rand.NextBool() ? ManosabaSounds.Ema_HitSound : ManosabaSounds.Ema_Kiang;
                    SoundEngine.PlaySound(hitSound, Player.Center);
                }
            }
            return true;
        }
        public override void UpdateVisibleVanityAccessories()
        {
            //只有暂停的时候下面才会调用这个方法
            bool isPaused = Main.gamePaused || Main.autoPause;
            //重写这段是为了开启自动暂停的时候，也能绘制需要的时装
            if (vanityEquipped && isPaused)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Head);
            }
        }
        public override void FrameEffects()
        {
            //这里只能通过遍历所有玩家原版盔甲栏的方式来寻找需要的时装物品。
            //因为这里需要实现的效果是，让人物存档页面也能绘制需要的时装
            //如果玩家佩戴了其他的饰品栏，那么……嗯，随便吧反正
            bool equip = false;
            if (Main.gameMenu)
                GetArmorSlotItem(ref equip);
            if (vanityEquipped || equip)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, nameof(SakurabaEmma), EquipType.Head);
            }
            //绘制粒子
            if (vanityEquipped)
            {
                if (Player.IsStandingStil(5f))
                {
                    if (Timer <= 0f)
                        DrawGlow();
                }
                else
                {
                    if (Main.rand.NextBool())
                        DrawGeneralParticle();
                }
            }
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
        public override void PostUpdateMiscEffects()
        {
            if (Timer > 0f)
                Timer--;
        }

        private void DrawGeneralParticle()
        {
            //如果玩家速度过小，我们不生成粒子。
            Vector2 mountedPlayerPos = Player.position;
            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(new Rectangle((int)mountedPlayerPos.X, (int)mountedPlayerPos.Y, Player.width, Player.height));
            Vector2 vel = Player.velocity.SafeNormalize(Vector2.UnitX) * -Main.rand.NextFloat(0.3f, 1.25f) * 1.1f;
            new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(3), 0.6f, RandLerpColor(Color.HotPink, Color.LightPink), 40, 0.34f, RandRotTwoPi).Spawn();
            if(Main.rand.NextBool(3))
            new Petal(spawnPos, vel, RandLerpColor(Color.HotPink, Color.LightPink), 40, RandRotTwoPi, 1f, 0.1f, 0.5f).Spawn();
        }

        public void DrawGlow()
        {
            //这里的写法潜在问题是如果有人试图手动操作玩家的贴图大小，则无法校准
            //但是……我还真没见过这种情况。先不管反正。
            Timer = 120f;
            Vector2 spawnPos = Player.direction > 0 ? new Vector2(Player.position.X + 2, Player.position.Y + 2) : new Vector2(Player.position.X + 15, Player.position.Y + 2);
            new CrossGlow(spawnPos, Color.Pink, 30, 1, 0.10f).Spawn();
            for (int i = 0; i < 3; i++)
            {
                //花瓣与光球
                new Petal(spawnPos, Vector2.UnitY * Main.rand.NextFloat(1.1f, 1.3f), RandLerpColor(Color.HotPink, Color.LightPink), 120, RandRotTwoPi, 0.8f, Main.rand.NextFloat(0.08f, 0.1f), 0.3f).Spawn();
                new TurbulenceShinyOrb(spawnPos.ToRandCirclePosEdge(3), 0.2f, RandLerpColor(Color.HotPink, Color.LightPink), 120, 0.22f, RandRotTwoPi).Spawn();
            }

        }
    }
    /// <summary>
    /// 代办：将其扔到统一基类管理
    /// </summary>
    public class SakurabaEmma : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items";
        public static string ItemPath => "SakurabaEmaMod/Assets/Texture/CharactorSets/SakurabaEma/";
        public override string Texture => $"{ItemPath}Item";
        //没有理由给这个东西敲词缀，说实话
        public override bool AllowPrefix(int pre) => false;
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Head", EquipType.Head, this);
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Body", EquipType.Body, this);
            EquipLoader.AddEquipTexture(Mod, $"{ItemPath}Legs", EquipType.Legs, this);
        }
        public override void SetStaticDefaults()
        {

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }
        public override bool ConsumeItem(Player player) => false;
        public int Timer = 0;
        public int FlavorTooltipIndex = 0;
        public int RealTooltipIndex = 0;
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = RarityType<SakurabaEmaRarity>();
            Item.vanity = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 15).
                DisableDecraft().
                AddTile(TileID.Loom).
                Register();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //遍历一遍寻找需要绘制的特殊台词位置的索引
            //这里需要把对应的原罪名与原罪文本直接植入到物品名的下方，让其看起来比较协调
            FlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "ItemName" && line.Mod == "Terraria");
            //通过本地化路径获取相应的内容，一个是原罪名，第二个为原罪文本
            string value = this.GetLocalizedValue("FlavorTooltip").ToLangValue();
            string realTooltipValue = this.GetLocalizedValue("RealTooltip").ToLangValue();
            //实例化TooltipLine，这里的名字不能乱写，需要作为后面绘制特殊效果用的一个索引
            TooltipLine flavorTooltip = new TooltipLine(Mod, "FlavorTooltipName", value);
            TooltipLine realTooltip = new TooltipLine(Mod, "RealToolTipName", realTooltipValue);
            //植入Tooltip。
            tooltips.Insert(FlavorTooltipIndex + 1, flavorTooltip);
            tooltips.Insert(FlavorTooltipIndex + 2, realTooltip);
            //艾玛有中键音效切换，这里需要提示玩家当前使用的版本
            //直接用的封装CreateTooltip方法
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<SakurabaEmaPlayer>().JustKiang)
            {
                tooltips.CreateTooltip(this.GetLocalizedValue("KiangSound"), LineName: "SoundName");
            }
            else
                tooltips.CreateTooltip(this.GetLocalizedValue("RegularSound"), LineName: "SoundName");
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            //为物品名本身绘制特效
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                SakurabaEmaRarity.DrawRarity(line);
                return false;
            }
            //为原罪名文本绘制特效
            if (line.Mod == Mod.Name && line.Name == "FlavorTooltipName")
            {
                SakurabaEmaRarity.DrawFlavor(line);
                return false;
            }
            //为原罪文本绘制特效
            if (line.Mod == Mod.Name && line.Name == "RealToolTipName")
            {
                SakurabaEmaRarity.DrawTooltip(line);
                return false;
            }
            if (line.IsThisLine("Vanity") || line.IsThisLine("Equipable"))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;
            }
            if (line.IsThisLine("Tooltip0"))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;

            }
            if (line.IsThisLine("SoundName", Mod.Name))
            {
                SakurabaEmaRarity.DrawMisc(line);
                return false;
            }
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            base.PostDrawTooltipLine(line);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = TextureAssets.Item[Type].Value;
            Vector2 position = Item.position - Main.screenPosition + tex.Size() / 2;
            Rectangle iFrame = tex.Frame();
            //绘制物品时装描边
            for (int i = 0; i < 16; i++)
                spriteBatch.Draw(tex, position + ToRadians(i * 60f).ToRotationVector2() * 2.4f, null, Color.Pink with { A = 0 }, 0f, tex.Size() / 2, scale, 0, 0f);
            //绘制物品本身
            spriteBatch.Draw(tex, position, iFrame, Color.White, 0f, tex.Size() / 2, scale, 0f, 0f);
            Lighting.AddLight(position, TorchID.UltraBright);
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = TextureAssets.Item[Type].Value;
            //描边。
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(tex, position + ToRadians(60f * i).ToRotationVector2() * 2.1f, frame, Color.LightPink.ToAddColor(), 0f, origin, scale, 0, 0);
            }
            //本身
            spriteBatch.Draw(tex, position, frame, Color.White, 0, origin, scale, 0, 0);
            return false;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<SakurabaEmaPlayer>().vanityEquipped = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SakurabaEmaPlayer>().vanityEquipped = !hideVisual;
        }
    }
}

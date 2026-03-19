using SakurabaEmaMod.Core;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Globals.Class
{
    public abstract class CharactorItem : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Items";
        /// <summary>
        /// 当前物品归属的人物
        /// 复写这个同样会尝试去寻找对应的材质。
        /// </summary>
        public virtual short SetCharactor { get; }
        /// <summary>
        /// 物品大小
        /// </summary>
        public virtual int Size => 24;
        /// <summary>
        /// 物品使用的稀有度
        /// </summary>
        public virtual int SetRarity => ManosabaRaritySystem.Instance.SetRarityType(SetCharactor);
        public string GetAsset(string path)
        {
            return $"SakurabaEmaMod/Assets/Texture/{path}/{GetType().Name}";
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = Size;
            Item.rare = SetRarity;
            ExSD();
        }
        public virtual void ExSD() { }
    }
}

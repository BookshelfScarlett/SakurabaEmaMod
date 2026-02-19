using SakurabaEmaMod.Globals.Enums;
using SakurabaEmaMod.Rarity.RarityShiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakurabaEmaMod.Rarity
{
    /// <summary>
    /// 按照原版写的，类似于RarityID的统一管理
    /// 主要是为了给那个抽象类包的饺子。
    /// </summary>
    public class ManoRarityID
    {
        public static int SakurabaEmaRarity = RarityType<SakuraRarity>();
        public static int JougasakiNoaRarity = -1;
        public Dictionary<Charactor, int> CharactorRarityDicionary = new Dictionary<Charactor, int>();
    }
}

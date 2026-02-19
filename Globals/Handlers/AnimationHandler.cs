using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakurabaEmaMod.Globals.Handlers
{
    public class AnimationHandler
    {
        public int[] AniProgress = [];

        public int[] MaxAniProgress = [];

        public float[] Auxfloat = [];

        public bool[] HasFinish = [];

        public float[] RotVelocity = [];

        public AnimationHandler(int TotalAniUnit)
        {
            // 使用 new int[length] 来创建指定长度的数组
            AniProgress = new int[TotalAniUnit];

            MaxAniProgress = new int[TotalAniUnit];

            Auxfloat = new float[TotalAniUnit];

            HasFinish = new bool[TotalAniUnit];

            RotVelocity = new float[TotalAniUnit];
        }
    }
}

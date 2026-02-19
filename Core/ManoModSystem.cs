using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Core
{
    public class ManoModSystem : ModSystem
    {
        public static Vector2 ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        public static Rectangle MouseRectangle;
        public override void UpdateUI(GameTime gameTime)
        {
            ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            MouseRectangle = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 4, 4);
        }
    }
}

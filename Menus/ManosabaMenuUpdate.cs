using SakurabaEmaMod.Core.Hud;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SakurabaEmaMod.Menus
{
    /// <summary>
    /// 统一的更新管理。
    /// </summary>
    public class ManosabaMenuUpdate
    {
        public static ManosabaHud LoadGame => ManoHudManager.UICollection[GetInstance<LoadGame>().Type];
        public static ManosabaHud Options => ManoHudManager.UICollection[GetInstance<Options>().Type];
        public static ManosabaHud Gallery => ManoHudManager.UICollection[GetInstance<Gallery>().Type];
        public static ManosabaHud Exit => ManoHudManager.UICollection[GetInstance<Exit>().Type];
        public static ManosabaHud SwitchMenu => ManoHudManager.UICollection[GetInstance<SwitchMenu>().Type];
        public static void CustomUpdate()
        {
            if (Main.menuMode == ManosabaMenu.ID)
            {
                LoadGame.Update();
                Options.Update();
                Gallery.Update();
                Exit.Update();
                SwitchMenu.Update();
            }

        }

    }
}

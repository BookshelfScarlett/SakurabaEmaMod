global using static Microsoft.Xna.Framework.MathHelper;
global using static SakurabaEmaMod.Globals.Handlers.GlobalHandlers;
global using static SakurabaEmaMod.Globals.Handlers.EasingHandler;
global using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SakurabaEmaMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SakurabaEmaMod : Mod
	{
        public static SakurabaEmaMod Instance;
        public static Mod CrossMod_FuckEmma = null;
        public override void Load()
        {
            Instance = this;
            ModLoader.TryGetMod("Sounds_SakurabaEma", out CrossMod_FuckEmma);
        }
        public override void Unload()
        {
            Instance = null;
            CrossMod_FuckEmma = null;
        }

    }
}

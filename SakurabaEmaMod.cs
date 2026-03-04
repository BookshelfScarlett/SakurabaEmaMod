global using static Microsoft.Xna.Framework.MathHelper;
global using static SakurabaEmaMod.Globals.Handlers.EasingHandler;
global using static SakurabaEmaMod.Globals.Handlers.GlobalHandlers;
global using static Terraria.ModLoader.ModContent;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using ReLogic.Content.Sources;
using SakurabaEmaMod.Assets.Register;
using SakurabaEmaMod.Core.Readers.Ogv;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SakurabaEmaMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SakurabaEmaMod : Mod
	{
        public static SakurabaEmaMod Instance;
        public static Mod CrossMod_FuckEmma = null;
        public const int EmaSoundID = 1;
        public override IContentSource CreateDefaultContentSource()
        {
            if(!Main.dedServ)
            {
                AddContent(new OgvReader());
            }
            return base.CreateDefaultContentSource();
        }
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
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            base.HandlePacket(reader, whoAmI);
            int packedID = reader.ReadInt32();
            if(packedID == EmaSoundID)
            {
                Vector2 pos = reader.ReadVector2();
                int soundType = reader.ReadInt32();
                SoundStyle playSound = soundType == 0 ? ManosabaSounds.Ema_HitSound : ManosabaSounds.Ema_Kiang;
                SoundEngine.PlaySound(playSound, pos);
            }
        }

    }
}

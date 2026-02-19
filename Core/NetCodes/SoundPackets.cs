using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Net;

namespace SakurabaEmaMod.Core.NetCodes
{
    public static class SoundPackets
    {
        public static void BroadcastSound(Vector2 pos, int soundType)
        {
            var mod = GetInstance<SakurabaEmaMod>();
            ModPacket packet = mod.GetPacket();
            packet.Write(SakurabaEmaMod.EmaSoundID);
            packet.WriteVector2(pos);
            packet.Write(soundType);
            packet.Send();
        }
    }
}

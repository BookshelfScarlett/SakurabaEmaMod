using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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

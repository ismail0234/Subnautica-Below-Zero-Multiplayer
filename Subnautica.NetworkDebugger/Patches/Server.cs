namespace Subnautica.NetworkDebugger.Patches
{
    using System;

    using HarmonyLib;

    using LiteNetLib;

    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Models.Core;
    using Subnautica.NetworkDebugger.Data;
    using Subnautica.Network.Extensions;

    [HarmonyPatch]
    public class Server
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Subnautica.Server.Core.ServerListener), nameof(Subnautica.Server.Core.ServerListener.OnNetworkReceive))]
        private static void OnNetworkReceive(Subnautica.Server.Core.ServerListener __instance, NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if (Network.IsMultiplayerActive && NetworkDebuggerBehaviour.Instance.IsActive)
            {
                var result = NetworkTools.Deserialize<NetworkPacket>(new ArraySegment<byte>(reader.RawData, reader.Position, reader.AvailableBytes));

                NetworkDebuggerBehaviour.Instance.AddPacketLog(reader.AvailableBytes, result.ChannelType, deliveryMethod, false, false);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Subnautica.Server.Core.Server), nameof(Subnautica.Server.Core.Server.SendPacket), new Type[] { typeof(string), typeof(NetworkPacket) })]
        private static void SendPacket(ref bool __result, string ipPort, NetworkPacket packet)
        {
            if (Network.IsMultiplayerActive && NetworkDebuggerBehaviour.Instance.IsActive && __result)
            {
                NetworkDebuggerBehaviour.Instance.AddPacketLog(packet.Serialize().Length, packet.ChannelType, packet.DeliveryMethod, true, false);
            }
        }
    }
}

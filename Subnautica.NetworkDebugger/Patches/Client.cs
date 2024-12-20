namespace Subnautica.NetworkDebugger.Patches
{
    using System;

    using HarmonyLib;

    using LiteNetLib;

    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Network.Core;
    using Subnautica.Network.Models.Core;
    using Subnautica.NetworkDebugger.Data;
    using Subnautica.Network.Extensions;

    using UnityEngine;

    [HarmonyPatch]
    public class Client
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientListener), nameof(ClientListener.OnNetworkReceive))]
        private static void OnNetworkReceive(ClientListener __instance, NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if (Network.IsMultiplayerActive && NetworkDebuggerBehaviour.Instance.IsActive)
            {
                var result = NetworkTools.Deserialize<NetworkPacket>(new ArraySegment<byte>(reader.RawData, reader.Position, reader.AvailableBytes));

                NetworkDebuggerBehaviour.Instance.AddPacketLog(reader.AvailableBytes, result.ChannelType, deliveryMethod, true, true);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(NetworkClient), nameof(NetworkClient.SendPacket))]
        private static void SendPacket(ref bool __result, NetworkPacket packet)
        {
            if (Network.IsMultiplayerActive && NetworkDebuggerBehaviour.Instance.IsActive && __result)
            {
                NetworkDebuggerBehaviour.Instance.AddPacketLog(packet.Serialize().Length, packet.ChannelType, packet.DeliveryMethod, false, true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Player), nameof(global::Player.Update))]
        private static void Update()
        {
            if (Network.IsMultiplayerActive && Input.GetKeyDown(KeyCode.F5))
            {
                if (NetworkDebuggerBehaviour.Instance.IsActive)
                {
                    NetworkDebuggerBehaviour.Hide();
                }
                else
                {
                    NetworkDebuggerBehaviour.Show();
                }
            }
        }
    }
}

namespace Subnautica.Client.Synchronizations.Processors.World
{
    using System.Collections;
    using System.Collections.Generic;

    using Oculus.Platform;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.API.Features.NetworkUtility;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class PrecursorTeleporterProcessor : NormalProcessor
    {
        /**
         *
         * Pasif Işınlayıcıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<string, string> DisabledTeleporters = new Dictionary<string, string>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.PrecursorTeleporterArgs>();
            if (packet == null)
            {
                return false;
            }

            if (packet.IsTerminal)
            {
                this.ActivateTargetTeleporter(packet.TeleporterId, packet.UniqueId);

                var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
                if (player == null)
                {
                    return false;
                }

                if (player.IsMine)
                {
                    player.OnHandClickPrecursorTerminal(packet.UniqueId);
                }
                else
                {
                    player.ActivatePrecursorTerminal(packet.UniqueId);
                }
            }
            else
            {
                var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
                if (player != null)
                {
                    if (packet.IsTeleportStart)
                    {
                        player.Hide(false);
                    }
                    else
                    {
                        player.Show(false);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Hedef geçidi aktif eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ActivateTargetTeleporter(string teleporterId, string uniqueId)
        {
            TeleporterManager.main.activeTeleporters.Add(teleporterId);
            TeleporterManager.main.activeTeleporters.Add(uniqueId);

            if (DisabledTeleporters.TryGetValue(teleporterId, out var targetTeleporterId))
            {
                DisabledTeleporters.Remove(teleporterId);

                UWE.CoroutineHost.StartCoroutine(this.ActivateTargetTeleporterAsync(targetTeleporterId));
            }
        }

        /**
         *
         * Hedef geçidi ASYNC aktif eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerator ActivateTargetTeleporterAsync(string targetTeleporterId)
        {
            yield return new WaitForSecondsRealtime(4f);

            var teleporter = Network.Identifier.GetComponentByGameObject<global::PrecursorTeleporter>(targetTeleporterId, true);
            if (teleporter)
            {
                teleporter.ToggleDoor(true);
            }
        }

        /**
         *
         * Işınlayıcı başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleporterInitialized(TeleporterInitializedEventArgs ev)
        {
            if (ev.IsExit)
            {
                if (TeleporterManager.GetTeleporterActive(ev.TeleporterId))
                {
                    DisabledTeleporters.Remove(ev.TeleporterId);
                }
                else
                {
                    DisabledTeleporters[ev.TeleporterId] = ev.UniqueId;
                }
            }
        }

        /**
         *
         * Oyuncu ışınlanma başladıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPrecursorTeleporterUsed()
        {
            PrecursorTeleporterProcessor.SendPacketToServer(isTeleportStart: true);
        }

        /**
         *
         * Oyuncu ışınlanma tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPrecursorTeleportationCompleted()
        {
            PrecursorTeleporterProcessor.SendPacketToServer(isTeleportCompleted: true);
        }

        /**
         *
         * Işınlayıcı terminali aktif edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleporterTerminalActivating(TeleporterTerminalActivatingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId))
            {
                PrecursorTeleporterProcessor.SendPacketToServer(ev.UniqueId, ev.TeleporterId, true);
            }
        }

        /**
         *
         * Bir nesne yok edildiğinde içinden başka nesne çıkarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId = null, string teleporterId = null, bool isTerminal = false, bool isTeleportStart = false, bool isTeleportCompleted = false)
        {
            ServerModel.PrecursorTeleporterArgs request = new ServerModel.PrecursorTeleporterArgs()
            {
                UniqueId            = uniqueId,
                TeleporterId        = teleporterId,
                IsTerminal          = isTerminal,
                IsTeleportStart     = isTeleportStart,
                IsTeleportCompleted = isTeleportCompleted,
            };

            NetworkClient.SendPacket(request);
        }

        /**
         *
         * Ana menüye dönünce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnDispose()
        {
            DisabledTeleporters.Clear();
        }
    }
}
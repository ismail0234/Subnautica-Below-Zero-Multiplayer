namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class ScannerProcessor : PlayerItemProcessor
    {
        /**
         *
         * Oyuncuların tarama hedeflerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, string> PlayerTargetIds { get; set; } = new Dictionary<byte, string>();

        /**
         *
         * Oyuncuların tarama hedef zamanlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, float> PlayerTargetTimes { get; set; } = new Dictionary<byte, float>();

        /**
         *
         * Oyuncuların tarama hedef zamanlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<byte> PlayerRemovingIds { get; set; } = new List<byte>();

        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPlayerItemComponent packet, byte playerId)
        {
            var entity = packet.GetComponent<ItemModel.Scanner>();
            if (entity.TargetId.IsNull())
            {
                return false;
            }

            this.PlayScanFX(playerId, entity.TargetId);
            return true;
        }

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.PlayerTargetIds.Clear();
            this.PlayerTargetTimes.Clear();
            this.PlayerRemovingIds.Clear();
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            if (World.IsLoaded)
            {
                float currentTime = Time.time;

                foreach (var item in this.PlayerTargetTimes)
                {
                    if (currentTime >= item.Value + 0.25f)
                    {
                        this.PlayerRemovingIds.Add(item.Key);
                        this.StopScanFX(item.Key);
                    }
                }

                if (this.PlayerRemovingIds.Any())
                {
                    foreach (var playerId in this.PlayerRemovingIds)
                    {
                        this.PlayerTargetIds.Remove(playerId);
                        this.PlayerTargetTimes.Remove(playerId);
                    }

                    this.PlayerRemovingIds.Clear();
                }
            }
        }

        /**
         *
         * Taramayı Başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool PlayScanFX(byte playerId, string targetId)
        {
            if (this.PlayerTargetIds.TryGetValue(playerId, out string tempTargetId))
            {
                this.PlayerTargetTimes[playerId] = Time.time;

                if (tempTargetId == targetId)
                {
                    return false;
                }

                this.StopScanFX(playerId);
            }

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player == null || player.TechTypeInHand != TechType.Scanner)
            {
                return false;
            }

            var tool = this.GetPlayerTool<global::ScannerTool>(player, TechType.Scanner);
            if (tool == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetGameObject(targetId, true);
            if (gameObject == null)
            {
                return false;
            }

            this.PlayerTargetIds[playerId]   = targetId;
            this.PlayerTargetTimes[playerId] = Time.time;

            tool.scanBeam.SetActive(true);

            var scanFX = gameObject.EnsureComponent<VFXOverlayMaterial>();
            if (gameObject.GetComponent<global::Creature>() != null)
            {
                scanFX.ApplyOverlay(tool.scanMaterialOrganicFX, "VFXOverlay: Scanning", false);
            }
            else
            {
                scanFX.ApplyOverlay(tool.scanMaterialCircuitFX, "VFXOverlay: Scanning", false);
            }

            return true;
        }

        /**
         *
         * Taramayı Durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool StopScanFX(byte playerId)
        {
            if (!this.PlayerTargetIds.TryGetValue(playerId, out string targetId))
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player != null && player.TechTypeInHand == TechType.Scanner)
            {
                var tool = this.GetPlayerTool<global::ScannerTool>(player, TechType.Scanner);
                if (tool)
                {
                    tool.scanBeam.SetActive(false);
                }
            }

            var gameObject = Network.Identifier.GetGameObject(targetId, true);
            if (gameObject == null)
            {
                return false;
            }

            var scanFX = gameObject.EnsureComponent<VFXOverlayMaterial>();
            if (scanFX != null)
            {
                scanFX.RemoveOverlay();
            }

            return true;
        }

        /**
         *
         * Oyuncu bir nesneyi tararken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScannerUsing(ScannerUsingEventArgs ev)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Scanner()
                {
                    TargetId = ev.UniqueId
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}

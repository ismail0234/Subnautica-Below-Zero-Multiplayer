namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ExosuitDrillProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.ExosuitDrillArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            if (packet.IsStaticWorldEntity)
            {
                Network.StaticEntity.AddStaticEntity(packet.StaticEntity);

                var drillable = Network.Identifier.GetComponentByGameObject<global::Drillable>(packet.SlotId, true);
                if (drillable != null)
                {
                    drillable.SetHealth(packet.NewHealth, isSpawnFx: true);
                }
            }
            else
            {
                var spawnPoint = Network.WorldStreamer.GetSlotById(packet.SlotId.WorldStreamerToSlotId());
                if (spawnPoint == null)
                {
                    return false;
                }

                spawnPoint.SetHealth(packet.NewHealth);

                var component = spawnPoint.GetComponent();
                if (component)
                {
                    component.DrillableHealthSync(packet.InventoryItems.Count > 0 || packet.WorldItems.Count > 0);
                }
                
                if (packet.DisableItem != null)
                {
                    World.DestroyPickupItem(packet.DisableItem);
                }
            }

            foreach (var item in packet.InventoryItems)
            {
                Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), item);
            }

            foreach (var entity in packet.WorldItems)
            {
                Network.DynamicEntity.Spawn(entity, this.OnEntitySpawned);
            }

            if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
            {
                foreach (var item in packet.InventoryItems)
                {
                    item.Item.TechType.ShowPickupNotify();

                    ErrorMessage.AddMessage(global::Language.main.GetFormat<string>("VehicleAddedToStorage", global::Language.main.Get(item.Item.TechType.GetTechName())));
                }

                for (int i = 0; i < packet.WorldItems.Count; i++)
                {
                    ErrorMessage.AddMessage(global::Language.main.Get("ContainerCantFit"));
                }
            }

            return true;
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerDrop();
        }

        /**
         *
         * Exosuit ile maden kazarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExosuitDrilling(ExosuitDrillingEventArgs ev)
        {
            ev.IsAllowed = false;

            ExosuitDrillProcessor.SendPacketToServer(ev.UniqueId, ev.SlotId, ev.MaxHealth, ev.DropTechType, ev.DropPositions, ev.IsMultipleDrill, ev.IsStaticWorldEntity);
        }

        /**
         *
         * Spy Penguin bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string slotId, float maxHealth, TechType dropTechType, List<ZeroVector3> dropPositions, bool isMultipleDrill, bool isStaticWorldEntity)
        {
            ServerModel.ExosuitDrillArgs request = new ServerModel.ExosuitDrillArgs()
            {
                UniqueId            = uniqueId,
                SlotId              = slotId,
                MaxHealth           = maxHealth,
                DropTechType        = dropTechType,
                DropPositions       = dropPositions,
                IsMultipleDrill     = isMultipleDrill,
                IsStaticWorldEntity = isStaticWorldEntity,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;

    using UnityEngine;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckSleeperModuleProcessor : WorldDynamicEntityProcessor
    {
        /**
         *
         * Dünya yüklenip nesne doğduğunda çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnWorldLoadItemSpawn(NetworkDynamicEntityComponent packet, bool isDeployed, Pickupable pickupable, GameObject gameObject)
        {
            if (!isDeployed)
            {
                return false;
            }

            gameObject.SetActive(true);

            var component = packet.GetComponent<WorldEntityModel.SeaTruckSleeperModule>();
            if (component == null)
            {
                return false;
            }

            if (component.Bed.IsUsing())
            {
                MultiplayerChannelProcessor.AddPacketToProcessor(NetworkChannel.StartupWorldLoaded, this.GetBedPacket(component.Bed.PlayerId_v2, gameObject.GetIdentityId(), component.Bed));
            }

            return true;
        }

        /**
         *
         * Uyku paketini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ServerModel.MetadataComponentArgs GetBedPacket(byte playerId, string uniqueId, BedSideItem bedSide)
        {
            return new ServerModel.MetadataComponentArgs()
            {
                PacketOwnerId = playerId,
                TechType      = TechType.Bed1,
                UniqueId      = uniqueId,
                Component     = new Metadata.Bed()
                {
                    IsSleeping  = true,
                    CurrentSide = bedSide,
                }
            };
        }
    }
}
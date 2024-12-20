namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System.Linq;
    using Steamworks;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;

    using UnityEngine;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckFabricatorModuleProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>();
            if (component == null)
            {
                return false;
            }

            foreach (var storageContainer in gameObject.GetComponentsInChildren<global::StorageContainer>())
            {
                var locker = component.Lockers.ElementAt(0);
                if (locker == null)
                {
                    continue;
                }

                Network.Identifier.SetIdentityId(storageContainer.gameObject, locker.UniqueId);

                foreach (var item in locker.StorageContainer.Items)
                {
                    Entity.SpawnToQueue(item.Item, item.ItemId, storageContainer.container);
                }
            }

            foreach (var coloredLabel in gameObject.GetComponentsInChildren<global::ColoredLabel>())
            {
                var locker = component.Lockers.ElementAt(0);
                if (locker == null)
                {
                    continue;
                }

                Network.Identifier.SetIdentityId(coloredLabel.signInput.gameObject, ZeroGame.GetSeaTruckColoredLabelUniqueId(locker.UniqueId));

                if (locker.StorageContainer.Sign != null)
                {
                    MetadataProcessor.ExecuteProcessor(TechType.Sign, ZeroGame.GetSeaTruckColoredLabelUniqueId(locker.UniqueId), locker.StorageContainer.Sign, true);
                }
            }

            var fabricator = gameObject.GetComponentInChildren<global::AddressablesPrefabSpawn>();
            if (fabricator)
            {
                Network.Identifier.SetIdentityId(fabricator.gameObject, component.FabricatorUniqueId);
            }

            var construction = Network.Session.Current.Constructions.FirstOrDefault(q => q.UniqueId == component.FabricatorUniqueId);
            if (construction?.Component != null)
            {
                MultiplayerChannelProcessor.AddPacketToProcessor(NetworkChannel.StartupWorldLoaded, this.GetFabricatorPacket(construction.UniqueId, construction.Component));
            }

            return true;
        }

        /**
         *
         * Fabricator paketini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ServerModel.MetadataComponentArgs GetFabricatorPacket(string uniqueId, MetadataComponent component)
        {
            return new ServerModel.MetadataComponentArgs()
            {
                TechType  = TechType.Fabricator,
                UniqueId  = uniqueId,
                Component = component
            };
        }
    }
}
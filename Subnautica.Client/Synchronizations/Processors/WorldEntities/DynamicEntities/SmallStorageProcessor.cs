namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SmallStorageProcessor : WorldDynamicEntityProcessor
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
            var component = packet.GetComponent<WorldEntityModel.SmallStorage>();
            if (component == null)
            {
                return false;
            }

            var storage = gameObject.GetComponent<global::DeployableStorage>();
            if (storage == null)
            {
                return false;
            }

            var pickupableStorage = gameObject.GetComponentInChildren<global::PickupableStorage>();
            if (pickupableStorage == null)
            {
                return false;
            }

            if (component.StorageContainer != null)
            {
                if (component.StorageContainer.Sign != null)
                {
                    using (EventBlocker.Create(TechType.Sign))
                    {
                        MetadataProcessor.ExecuteProcessor(TechType.Sign, Network.Identifier.GetIdentityId(gameObject), component.StorageContainer.Sign, true);
                    }
                }

                foreach (var item in component.StorageContainer.Items)
                {
                    Entity.SpawnToQueue(item.Item, item.ItemId, pickupableStorage.storageContainer.container);
                }
            }

            return true;
        }
    }
}
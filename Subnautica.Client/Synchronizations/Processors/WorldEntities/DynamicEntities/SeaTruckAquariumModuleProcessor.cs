namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckAquariumModuleProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.SeaTruckAquariumModule>();
            if (component == null)
            {
                return false;
            }

            int lockerIndex = 0;
            foreach (var storageContainer in gameObject.GetComponentsInChildren<global::StorageContainer>())
            {
                var locker = component.Lockers.ElementAt(lockerIndex++);
                if (locker == null)
                {
                    continue;
                }

                Network.Identifier.SetIdentityId(storageContainer.gameObject, locker.UniqueId);

                foreach (var item in locker.StorageContainer.Items)
                {
                    Entity.SpawnToQueue(item.TechType, item.ItemId, storageContainer.container);
                }
            }

            return true;
        }
    }
}
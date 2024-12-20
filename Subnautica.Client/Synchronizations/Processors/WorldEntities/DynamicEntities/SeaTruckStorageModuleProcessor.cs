namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckStorageModuleProcessor : WorldDynamicEntityProcessor
    {
        /**
         *
         * ColoredLabelIndex Değerleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<int> ColoredLabelIndex = new List<int>()
        {
            2,
            3,
            0,
            4,
            1
        };

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

            var component = packet.GetComponent<WorldEntityModel.SeaTruckStorageModule>();
            if (component == null)
            {
                return false;
            }

            foreach (var storageContainer in gameObject.GetComponentsInChildren<global::StorageContainer>())
            {
                var locker = component.Lockers.ElementAt(this.GetStorageContainerIndex(storageContainer.name));
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
                var indexId = this.GetStorageContainerIndex(coloredLabel.name);
                var locker  = component.Lockers.ElementAt(this.ColoredLabelIndex.ElementAt(indexId));
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

            return true;
        }

        /**
         *
         * StorageContainer index değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int GetStorageContainerIndex(string name)
        {
            if (!name.Contains("("))
            {
                return 0;
            }

            var data = name.Split('(');
            return Convert.ToInt32(data[1].Substring(0, 1));
        }
    }
}
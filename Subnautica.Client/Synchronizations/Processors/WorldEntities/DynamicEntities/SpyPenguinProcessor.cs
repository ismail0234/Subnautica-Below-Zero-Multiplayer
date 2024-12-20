namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SpyPenguinProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.SpyPenguin>();
            if (component == null)
            {
                return false;
            }

            var spyPenguin = pickupable.GetComponent<SpyPenguinPlayerTool>();
            if (spyPenguin == null)
            {
                return false;
            }

            spyPenguin.pickupable.MultiplayerDrop();
            spyPenguin.SetEquipped(false);
            spyPenguin.spyPenguin.SetInterationState();
            spyPenguin.isDeployed = true;

            if (spyPenguin.TryGetComponent<SpyPenguinName>(out var spyPenguinName))
            {
                spyPenguinName.savedName = component.Name;
                spyPenguinName.SetName();
            }

            if (component.StorageContainer != null)
            {
                foreach (var item in component.StorageContainer.Items)
                {
                    if (item.Item != null)
                    {
                        Entity.SpawnToQueue(item.Item, item.ItemId, spyPenguin.spyPenguin.inventory.container);
                    }
                    else
                    {
                        Entity.SpawnToQueue(item.TechType, item.ItemId, spyPenguin.spyPenguin.inventory.container);
                    }
                }
            }

            Vehicle.ApplyLiveMixin(spyPenguin.spyPenguin.liveMixin, component.LiveMixin.Health);
            return true;
        }
    }
}
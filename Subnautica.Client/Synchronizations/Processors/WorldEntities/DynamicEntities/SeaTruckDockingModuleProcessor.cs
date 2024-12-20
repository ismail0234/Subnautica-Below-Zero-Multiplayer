namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;

    using UnityEngine;
    
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckDockingModuleProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.SeaTruckDockingModule>();
            if (component == null)
            {
                return false;
            }

            if (component.IsDocked())
            {
                Vehicle.CraftVehicle(component.Vehicle, notify: false, onCompleted: this.OnSpawnedVehicle, customProperty: gameObject);
            }

            return true;
        }

        /**
         *
         * Araç üretildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpawnedVehicle(WorldDynamicEntity entity, ItemQueueAction item, GameObject gameObject)
        {
            WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, entity.IsDeployed, null, gameObject);

            if (item.GetProperty<GameObject>("CustomProperty").TryGetComponent<MultiplayerSeaTruckDockingBay>(out var dockingBay))
            {
                dockingBay.StartDocking(gameObject.GetIdentityId(), false, true);
            }
        }
    }
}
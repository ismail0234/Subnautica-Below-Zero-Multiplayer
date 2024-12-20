namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.SeaTruck>();
            if (component == null)
            {
                return false;
            }

            var vehicle = gameObject.GetComponentInChildren<global::SeaTruckMotor>();
            if (vehicle == null)
            {
                return false;
            }

            var uniqueId = Network.Identifier.GetIdentityId(gameObject);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            Vehicle.ApplyModules(component.Modules, vehicle.upgrades.modules, TechType.SeaTruck);
            Vehicle.ApplyBatterySlotIds(gameObject, TechType.SeaTruck, component.PowerCells.ElementAt(0).UniqueId, component.PowerCells.ElementAt(1).UniqueId);
            Vehicle.ApplyPowerCells(uniqueId, component.PowerCells);
            Vehicle.ApplyLights(gameObject.GetComponent<global::SeaTruckLights>(), component.IsLightActive);
            Vehicle.ApplyLiveMixin(vehicle.liveMixin, component.LiveMixin.Health);
            Vehicle.ApplyColorCustomizer(component.ColorCustomizer, vehicle.truckSegment.colorNameControl);
            return true;
        }
    }
}
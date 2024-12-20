namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class MapRoomCameraProcessor : WorldDynamicEntityProcessor
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
            pickupable.MultiplayerDrop();

            if (!isDeployed)
            {
                return false;
            }

            var component = packet.GetComponent<WorldEntityModel.MapRoomCamera>();
            if (component == null)
            {
                return false;
            }

            var mapRoomCamera = pickupable.GetComponent<MapRoomCamera>();
            if (mapRoomCamera == null)
            {
                return false;
            }

            mapRoomCamera.energyMixin.battery.charge = component.Battery.Charge;
            mapRoomCamera.lightsParent.SetActive(component.IsLightEnabled);

            Vehicle.ApplyLiveMixin(mapRoomCamera.liveMixin, component.LiveMixin.Health);
            return true;
        }
    }
}
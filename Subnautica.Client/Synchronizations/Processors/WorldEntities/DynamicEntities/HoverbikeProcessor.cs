namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class HoverbikeProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.Hoverbike>();
            if (component == null)
            {
                return false;
            }

            var vehicle  = gameObject.GetComponentInChildren<global::Hoverbike>();
            if (vehicle == null)
            {
                return false;
            }

            pickupable?.MultiplayerDrop();

            Vehicle.ApplyHoverbikeComponent(gameObject, component);
            Vehicle.ApplyLiveMixin(vehicle.liveMixin, component.LiveMixin.Health);
            return true;
        }
    }
}
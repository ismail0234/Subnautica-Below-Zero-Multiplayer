namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class FlareProcessor : WorldDynamicEntityProcessor
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
            
            var component = packet.GetComponent<WorldEntityModel.Flare>();
            if (component == null)
            {
                return false;
            }

            pickupable.MultiplayerDrop();

            if (pickupable.TryGetComponent<global::Flare>(out var flare))
            {
                flare.SetFlareActiveState(true);

                flare.flareActivateTime   = component.ActivateTime;
                flare.energyLeft          = component.Energy;
                flare.capRenderer.enabled = true;

                if (flare.fxControl && !flare.fxIsPlaying)
                {
                    flare.fxControl.Play(1);
                    flare.fxIsPlaying   = true;
                    flare.light.enabled = true;
                }
            }

            return true;
        }
    }
}
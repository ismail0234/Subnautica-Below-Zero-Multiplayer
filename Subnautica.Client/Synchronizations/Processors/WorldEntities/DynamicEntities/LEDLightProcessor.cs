namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    public class LEDLightProcessor : WorldDynamicEntityProcessor
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

            pickupable.MultiplayerDrop();

            if (pickupable.TryGetComponent<global::LEDLight>(out var tool))
            {
                tool.OnPlace();
            }

            return true;
        }
    }
}
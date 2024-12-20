namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    public class SeaTruckTeleportationModuleProcessor : WorldDynamicEntityProcessor
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

            return true;
        }
    }
}
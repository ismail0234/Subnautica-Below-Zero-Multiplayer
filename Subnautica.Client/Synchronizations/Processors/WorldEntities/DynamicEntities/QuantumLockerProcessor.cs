namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System.ComponentModel;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    public class QuantumLockerProcessor : WorldDynamicEntityProcessor
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
            var quantumLocker = gameObject.GetComponent<global::QuantumLocker>();
            if (quantumLocker)
            {
                quantumLocker.storageContainer.SetContainer(QuantumLockerStorage.main.storageContainer.container);
                quantumLocker.loadedFromSaveGame = true;

                QuantumLockerStorage.Register(quantumLocker);
            }

            return true;
        }
    }
}
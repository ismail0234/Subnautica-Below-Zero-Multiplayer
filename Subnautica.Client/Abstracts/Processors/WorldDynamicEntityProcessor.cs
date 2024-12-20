namespace Subnautica.Client.Abstracts.Processors
{
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    public abstract class WorldDynamicEntityProcessor : BaseProcessor
    {
        /**
         *
         * Dünya yüklenip nesne doğduğunda çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool OnWorldLoadItemSpawn(NetworkDynamicEntityComponent packet, bool isDeployed, Pickupable pickupable, GameObject gameObject);

        /**
         *
         * İşlemi yönlendirip çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ExecuteItemSpawnProcessor(TechType techType, NetworkDynamicEntityComponent packet, bool isDeployed, Pickupable pickupable, GameObject gameObject, bool isDrop = false)
        {
            if (ProcessorShared.WorldDynamicEntityProcessors.TryGetValue(techType, out var processor))
            {
                processor.OnWorldLoadItemSpawn(packet, isDeployed, pickupable, gameObject);
                return true;
            }
            else
            {
                Log.Error(string.Format("WorldDynamicEntityProcessor Not Found: {0}", techType));
                return false;
            }
        }
    }
}
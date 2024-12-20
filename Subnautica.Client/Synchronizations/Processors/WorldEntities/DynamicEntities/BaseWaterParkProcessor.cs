namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class BaseWaterParkProcessor : WorldDynamicEntityProcessor
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
            Log.Info("BaseWaterParkProcessor: " + isDeployed + ", active: " + gameObject.activeSelf);
            if (!isDeployed)
            {
                return false;
            }

            var component = packet.GetComponent<WorldEntityModel.WaterParkCreature>();
            if (component == null)
            {
                return false;
            }

            pickupable.MultiplayerDrop(waterParkId: component.WaterParkId, waterParkAddTime: component.AddedTime);
            Log.Info("BaseWaterParkProcessor 5: " + isDeployed + ", active: " + gameObject.activeSelf);
            return true;
        }
    }
}
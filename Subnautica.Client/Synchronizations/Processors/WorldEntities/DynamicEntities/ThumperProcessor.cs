namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class ThumperProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.Thumper>();
            if (component == null)
            {
                return false;
            }

            var tool = pickupable.GetComponent<global::Thumper>();
            if (tool == null)
            {
                return false;
            }

            pickupable.MultiplayerDrop();

            if (component.Charge == -1f)
            {
                using (EventBlocker.Create(TechType.Thumper))
                {
                    tool.transform.position = component.Position.ToVector3();
                    tool.placementPos       = tool.transform.position;
                    tool.Deploy(true);
                }
            }
            else
            {
                Vehicle.ApplyEnergyMixin(tool.GetComponent<EnergyMixin>(), component.Charge, () =>
                {
                    using (EventBlocker.Create(TechType.Thumper))
                    {
                        tool.transform.position = component.Position.ToVector3();
                        tool.placementPos       = tool.transform.position;
                        tool.Deploy(true);
                    }
                });
            }

            return true;
        }
    }
}
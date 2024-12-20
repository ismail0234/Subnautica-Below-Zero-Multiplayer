namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class BeaconProcessor : WorldDynamicEntityProcessor
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

            var component = packet.GetComponent<WorldEntityModel.Beacon>();
            if (component == null)
            {
                return false;
            }

            pickupable.MultiplayerDrop();

            if (gameObject.TryGetComponent<global::Beacon>(out var beacon))
            {
                using (EventBlocker.Create(TechType.Beacon))
                {
                    beacon.beaconLabel.SetLabel(component.Text);
                }

                beacon.transform.eulerAngles = new Vector3(0f, beacon.transform.eulerAngles.y, 0f);
                beacon.SetBeaconActiveState(true);
                beacon.beaconLabel.OnDropped();

                if (component.IsDeployedOnLand)
                {
                    beacon.SetDeployedOnLand();
                }
                else
                {
                    beacon.SetDeployedInWater();
                }
            }

            return true;
        }

        /**
         *
         * Beacon'u ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void InitializeBeaconSignal(global::Pickupable pickupable, Vector3 position, Quaternion rotation, string text, bool isDeployedOnLand)
        {
            pickupable.Activate(false);
            pickupable.attached = false;
            pickupable.transform.position = position;
            pickupable.transform.rotation = rotation;

            if (pickupable.TryGetComponent<global::Beacon>(out var beacon))
            {
                using (EventBlocker.Create(TechType.Beacon))
                {
                    beacon.beaconLabel.SetLabel(text);
                }

                beacon.beaconLabel.OnDropped();

                if (isDeployedOnLand)
                {
                    beacon.SetDeployedOnLand();
                }
                else
                {
                    beacon.SetDeployedInWater();
                }
            }
        }
    }
}
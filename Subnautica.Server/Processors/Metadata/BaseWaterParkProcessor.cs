namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class BaseWaterParkProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            var component = packet.Component.GetComponent<Metadata.BaseWaterPark>();
            if (component == null)
            {
                return false;
            }

            var waterPark = construction.EnsureComponent<Metadata.BaseWaterPark>();
            if (waterPark == null)
            {
                return false;
            }

            if (component.Entity.TechType.IsCreatureEgg())
            {
                component.Entity.TechType = component.Entity.TechType.ToCreatureEgg();
            }

            if (component.ProcessType == Metadata.BaseWaterParkProcessType.ItemDrop)
            {
                var pickupItem = WorldPickupItem.Create(component.Entity.UniqueId, component.Entity.TechType, PickupSourceType.PlayerInventoryDrop);
                if (pickupItem == null)
                {
                    return false;
                }

                Log.Info("CREATURE EGG TYPE => " + component.Entity.TechType + ", COUNT: " + waterPark.Eggs.Count);

                if (component.Entity.TechType.IsCreatureEgg())
                {
                    if (waterPark.AddCreatureEgg(component.Entity.UniqueId) && Server.Instance.Logices.Storage.TryPickupToWorld(pickupItem, profile.InventoryItems, out var entity))
                    {
                        entity.SetPositionAndRotation(component.Entity.Position, component.Entity.Rotation);
                        entity.SetOwnership(profile.UniqueId);
                        entity.SetDeployed(true);
                        entity.SetComponent(new WaterParkCreature(Server.Instance.Logices.World.GetServerTimeAsDouble(), construction.UniqueId));

                        component.Entity = entity;

                        profile.SendPacketToAllClient(packet);
                    }
                }
                else if (component.Entity.TechType.IsCreature())
                {

                }
            }

            return true;
        }
    }
}

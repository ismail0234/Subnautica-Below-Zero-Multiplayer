namespace Subnautica.Server.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ItemModel        = Subnautica.Network.Models.Items;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class PipeSurfaceFloaterProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, PlayerItemActionArgs packet)
        {
            var component = packet.Item.GetComponent<ItemModel.PipeSurfaceFloater>();
            if (component == null)
            {
                return false;
            }

            if (component.IsSurfaceFloaterDeploy())
            {
                var worldPickupItem = WorldPickupItem.Create(component.UniqueId, TechType.PipeSurfaceFloater, PickupSourceType.PlayerInventoryDrop);

                if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
                {
                    entity.SetPositionAndRotation(component.Position, component.Rotation);
                    entity.SetOwnership(profile.UniqueId);
                    entity.SetDeployed(true);
                    entity.SetComponent(new WorldEntityModel.PipeSurfaceFloater());

                    component.Entity = entity;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.IsOxygenPipePlace())
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(component.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var surfaceFloater = entity.Component.GetComponent<WorldEntityModel.PipeSurfaceFloater>();
                if (surfaceFloater == null)
                {
                    return false;
                }

                if (profile.IsInventoryItemExists(component.PipeId) && surfaceFloater.AddOxygenPipe(component.PipeId, component.ParentId, component.Position))
                {
                    profile.RemoveInventoryItem(component.PipeId);
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.IsOxygenPipePickup())
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(component.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var surfaceFloater = entity.Component.GetComponent<WorldEntityModel.PipeSurfaceFloater>();
                if (surfaceFloater == null)
                {
                    return false;
                }

                var worldPickupItem = WorldPickupItem.Create(component.PipeId, TechType.Pipe, PickupSourceType.NoSource);

                if (surfaceFloater.Childrens.RemoveWhere(q => q.UniqueId == component.PipeId) > 0)
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(worldPickupItem, profile.InventoryItems))
                    {
                        component.PickupItem = worldPickupItem;

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}

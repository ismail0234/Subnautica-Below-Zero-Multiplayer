namespace Subnautica.Server.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ItemModel        = Subnautica.Network.Models.Items;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class FlareProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.Flare>();
            if (component == null)
            {
                return false;
            }

            var worldPickupItem = WorldPickupItem.Create(StorageItem.Create(packet.Item.UniqueId, packet.Item.TechType), PickupSourceType.PlayerInventoryDrop);

            if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
            {
                entity.SetPositionAndRotation(component.Position, component.Rotation);
                entity.SetOwnership(profile.UniqueId);
                entity.SetDeployed(true);
                entity.SetComponent(new WorldEntityModel.Flare(component.Energy, Server.Instance.Logices.World.GetServerTime()));

                component.Entity = entity;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

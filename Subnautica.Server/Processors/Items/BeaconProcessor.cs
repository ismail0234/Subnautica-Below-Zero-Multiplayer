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

    public class BeaconProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.Beacon>();
            if (component.IsTextChanged)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.Item.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var beacon = entity.Component.GetComponent<WorldEntityModel.Beacon>();
                beacon.SetText(component.Text);

                profile.SendPacketToOtherClients(packet);
            }
            else
            {
                var worldPickupItem = WorldPickupItem.Create(StorageItem.Create(component.UniqueId, TechType.Beacon), PickupSourceType.PlayerInventoryDrop);

                if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
                {
                    entity.SetPositionAndRotation(component.Position, component.Rotation);
                    entity.SetOwnership(profile.UniqueId);
                    entity.SetDeployed(true);
                    entity.SetComponent(new WorldEntityModel.Beacon(component.IsDeployedOnLand, component.Text));

                    component.Entity = entity;

                    profile.SendPacketToAllClient(packet);
                } 
            }

            return true;
        }
    }
}

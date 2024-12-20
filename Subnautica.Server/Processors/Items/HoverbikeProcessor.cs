namespace Subnautica.Server.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using UnityEngine;

    using ItemModel = Subnautica.Network.Models.Items;

    internal class HoverbikeProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.Hoverbike>();
            if (component == null)
            {
                return false;
            }

            var worldPickupItem = WorldPickupItem.Create(StorageItem.Create(packet.Item.UniqueId, TechType.Hoverbike), PickupSourceType.PlayerInventoryDrop);

            if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
            {
                entity.SetPositionAndRotation(component.Position, Quaternion.identity.ToZeroQuaternion());
                entity.SetOwnership(profile.UniqueId);
                entity.SetDeployed(true);
                entity.SetComponent(component.Component);

                component.Entity = entity;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

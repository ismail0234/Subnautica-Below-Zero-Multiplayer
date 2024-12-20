namespace Subnautica.Server.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using UnityEngine;

    using ItemModel        = Subnautica.Network.Models.Items;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SpyPenguinProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.SpyPenguin>();
            if (component.IsStalkerFur)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntityComponent<WorldEntityModel.SpyPenguin>(packet.Item.UniqueId);
                if (entity == null)
                {
                    return false;
                }
                
                if (Random.value < component.SpawnChance)
                {
                    component.WorldPickupItem = WorldPickupItem.Create(StorageItem.Create(TechType.SnowStalkerFur));

                    if (entity.StorageContainer.HasRoomFor(component.WorldPickupItem.Item) && entity.StorageContainer.AddItem(component.WorldPickupItem.Item))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (component.IsPickup)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntityComponent<WorldEntityModel.SpyPenguin>(packet.Item.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, entity.StorageContainer))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.IsDeploy)
            {
                var worldPickupItem = WorldPickupItem.Create(StorageItem.Create(packet.Item.UniqueId, packet.Item.TechType), PickupSourceType.PlayerInventoryDrop);

                if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
                {
                    entity.SetPositionAndRotation(component.Position, component.Rotation);
                    entity.SetOwnership(profile.UniqueId);
                    entity.SetDeployed(true);
                    entity.SetComponent(this.CreateComponent(component));

                    component.Entity = entity;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(component.UniqueId, profile.UniqueId))
                {
                    return false;
                }

                var entity = Server.Instance.Storages.World.GetDynamicEntityComponent<WorldEntityModel.SpyPenguin>(packet.Item.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                if (component.IsAdded)
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, entity.StorageContainer, profile.InventoryItems))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
                else
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, profile.InventoryItems, entity.StorageContainer))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Bileşen oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WorldEntityModel.SpyPenguin CreateComponent(ItemModel.SpyPenguin component)
        {
            return new WorldEntityModel.SpyPenguin()
            {
                Name      = component.Name,
                LiveMixin = new WorldEntityModel.Shared.LiveMixin(component.Health, 10f),
            };
        }
    }
}

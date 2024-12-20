namespace Subnautica.Server.Processors.Vehicle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using EntityModel      = Subnautica.Network.Models.WorldEntity;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class ExosuitDrillProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.ExosuitDrillArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var exosuit = Server.Instance.Storages.World.GetDynamicEntityComponent<WorldEntityModel.Exosuit>(packet.UniqueId);
            if (exosuit == null)
            {
                return false;
            }
            
            if  (packet.IsStaticWorldEntity)
            {
                if (!Server.Instance.Storages.World.IsPersistentEntityExists(packet.SlotId))
                {
                    Server.Instance.Storages.World.SetPersistentEntity(new EntityModel.Drillable() { UniqueId = packet.SlotId });
                }

                var drillable = Server.Instance.Storages.World.GetPersistentEntity<EntityModel.Drillable>(packet.SlotId);
                if (drillable == null)
                {
                    return false;
                }

                if (drillable.LiveMixin == null)
                {
                    drillable.LiveMixin = new LiveMixin(packet.MaxHealth, packet.MaxHealth);
                }

                if (drillable.LiveMixin.IsDead)
                {
                    return false;
                }

                drillable.LiveMixin.TakeDamage(packet.IsMultipleDrill ? 50f : 25f);

                if (drillable.LiveMixin.IsDead)
                {
                    drillable.DisableSpawn();
                }

                if (Server.Instance.Storages.World.SetPersistentEntity(drillable))
                {
                    packet.NewHealth    = drillable.LiveMixin.Health;
                    packet.StaticEntity = drillable;
                }
            }
            else
            {
                if (!packet.SlotId.IsWorldStreamer())
                {
                    return false;
                }

                var drillableId = packet.SlotId.WorldStreamerToSlotId();
                if (drillableId == 0)
                {
                    return false;
                }

                var entity = Server.Instance.Logices.WorldStreamer.GetSpawnPointById(drillableId);
                if (entity == null)
                {
                    return false;
                }
    
                if (entity.IsDead)
                {
                    if (entity.IsRespawnable(Server.Instance.Logices.World.GetServerTime()))
                    {
                        entity.SetHealth(packet.MaxHealth);
                    }
                    else
                    {
                        return false;
                    }
                }
        
                entity.TakeDamage(packet.IsMultipleDrill ? 50f : 25f, packet.MaxHealth);

                if (entity.IsDead)
                {
                    if (Server.Instance.Storages.World.DisableSlot(packet.SlotId))
                    {
                        packet.DisableItem = WorldPickupItem.Create(StorageItem.Create(packet.SlotId, TechType.None), PickupSourceType.EntitySlot);
                        packet.DisableItem.NextRespawnTime = Server.Instance.Storages.World.GetSlotNextRespawnTime(packet.SlotId);
                    }
                }

                packet.NewHealth = entity.Health;
            }

            if (packet.NewHealth % 200 == 0)
            {
                exosuit.ResizeStorageContainer();

                var dropAmount = this.GetDropAmount();

                for (int i = 0; i < dropAmount; i++)
                {
                    var pickupItem = WorldPickupItem.Create(StorageItem.Create(packet.DropTechType), PickupSourceType.StorageContainer);

                    if (Server.Instance.Logices.Storage.TryPickupItem(pickupItem, exosuit.StorageContainer))
                    {
                        packet.InventoryItems.Add(pickupItem);
                    }
                    else
                    {
                        var dynamicEntity = Server.Instance.Logices.World.CreateDynamicEntity(pickupItem.GetItemId(), pickupItem.Item.Item, pickupItem.Item.TechType, null, null, isDeployed: false);
                        if (dynamicEntity != null)
                        {
                            dynamicEntity.SetPositionAndRotation(this.GetDropPosition(packet.DropPositions, i), new ZeroQuaternion());
                            dynamicEntity.SetOwnership(profile.UniqueId);

                            packet.WorldItems.Add(dynamicEntity);
                        }
                    }
                }
            }

            profile.SendPacketToAllClient(packet);
            return true;
        }

        /**
         *
         * Rastgele düşecek miktarı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroVector3 GetDropPosition(List<ZeroVector3> positions, int i)
        {
            return i < positions.Count ? positions.ElementAt(i) : positions.ElementAt(0);
        }

        /**
         *
         * Rastgele düşecek miktarı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetDropAmount()
        {
            return Convert.ToByte(Tools.GetRandomInt(1, 3));
        }
    }
}
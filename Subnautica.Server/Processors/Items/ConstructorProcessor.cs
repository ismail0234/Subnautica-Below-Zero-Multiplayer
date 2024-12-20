namespace Subnautica.Server.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using ItemModel        = Subnautica.Network.Models.Items;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class ConstructorProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.Constructor>();
            if (component.IsEngageActive())
            {
                if (Server.Instance.Storages.World.GetDynamicEntity(packet.Item.UniqueId) == null)
                {
                    return false;
                }

                if (component.IsEngage())
                {
                    if (!Server.Instance.Logices.Interact.IsBlocked(packet.Item.UniqueId))
                    {
                        Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.Item.UniqueId, true);

                        profile.SendPacketToAllClient(packet);
                    }
                }
                else
                {
                    if (Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId, packet.Item.UniqueId))
                    {
                        Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.UseVehicleBay);

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }
            else if (component.CraftingTechType != TechType.None)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.Item.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var constructor = entity.Component.GetComponent<WorldEntityModel.Constructor>();
                if (constructor == null || !constructor.IsCraftable(Server.Instance.Logices.World.GetServerTime()))
                {
                    return false;
                }

                component.CraftingFinishTime = constructor.CraftingFinishTime = Server.Instance.Logices.World.GetServerTime() + this.GetDuration(component.CraftingTechType);

                var dynamicEntity = Server.Instance.Logices.World.CreateDynamicEntity(Subnautica.API.Features.Network.Identifier.GenerateUniqueId(), component.CraftingTechType, component.CraftingPosition, component.CraftingRotation, profile.UniqueId);
                if (dynamicEntity != null)
                {
                    component.Entity = dynamicEntity.SetComponent(this.GetEntityComponent(component.CraftingTechType));

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                var worldPickupItem = WorldPickupItem.Create(StorageItem.Create(component.UniqueId, TechType.Constructor), PickupSourceType.PlayerInventoryDrop);

                if (Server.Instance.Logices.Storage.TryPickupToWorld(worldPickupItem, profile.InventoryItems, out var entity))
                {
                    entity.SetPositionAndRotation(component.Position, new ZeroQuaternion());
                    entity.SetOwnership(profile.UniqueId);
                    entity.SetDeployed(true);
                    entity.SetComponent(new WorldEntityModel.Constructor());

                    component.Entity = entity;

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private NetworkDynamicEntityComponent GetEntityComponent(TechType techType)
        {
            switch (techType)
            {
                case TechType.SeaTruck:                     return new WorldEntityModel.SeaTruck().Initialize(this.OnEntityComponentInitialized);
                case TechType.Exosuit:                      return new WorldEntityModel.Exosuit().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckFabricatorModule:     return new WorldEntityModel.SeaTruckFabricatorModule().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckStorageModule:        return new WorldEntityModel.SeaTruckStorageModule().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckAquariumModule:       return new WorldEntityModel.SeaTruckAquariumModule().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckDockingModule:        return new WorldEntityModel.SeaTruckDockingModule().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckSleeperModule:        return new WorldEntityModel.SeaTruckSleeperModule().Initialize(this.OnEntityComponentInitialized);
                case TechType.SeaTruckTeleportationModule:  return new WorldEntityModel.SeaTruckTeleportationModule().Initialize(this.OnEntityComponentInitialized);
            }

            return null;
        }

        /**
         *
         * Komponent sınıfı oluşturulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityComponentInitialized(NetworkDynamicEntityComponent entityComponent)
        {
            if (entityComponent is WorldEntityModel.SeaTruckFabricatorModule)
            {
                var component = entityComponent.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>();
                if (component != null)
                {
                    Server.Instance.Storages.Construction.AddConstructionItem(new Network.Models.Storage.Construction.ConstructionItem()
                    {
                        IsStatic = true,
                        UniqueId = component.FabricatorUniqueId,
                        TechType = TechType.Fabricator,
                        ConstructedAmount = 1f,
                    });
                }
            }
        }

        /**
         *
         * Araç yapım süresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetDuration(TechType techType)
        {
            var duration = 3f;
            switch (techType)
            {
                case TechType.Exosuit:
                    duration = 10f;
                    break;
                case TechType.SeaTruck:
                case TechType.SeaTruckFabricatorModule:
                case TechType.SeaTruckStorageModule:
                case TechType.SeaTruckAquariumModule:
                case TechType.SeaTruckDockingModule:
                case TechType.SeaTruckSleeperModule:
                case TechType.SeaTruckTeleportationModule:
                    duration = 15f;
                    break;
            }

            return duration;
        }
    }
}

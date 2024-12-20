namespace Subnautica.Server.Logic
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class Storage : BaseLogic
    {
        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.PictureFramesSync();
        }
        
        /**
         *
         * PictureFrameSync dosyasını senkron yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void PictureFramesSync()
        {
            string[] pictureFrames = new string[Core.Server.Instance.Storages.PictureFrame.Storage.Images.Count];

            Core.Server.Instance.Storages.PictureFrame.Storage.Images.Keys.CopyTo(pictureFrames, 0);

            foreach (var constructionId in pictureFrames)
            {
                var isConstruction   = Core.Server.Instance.Storages.Construction.GetConstruction(constructionId);
                var isSeaTruckModule = Core.Server.Instance.Storages.World.GetDynamicEntity(constructionId);
                if (isConstruction == null && isSeaTruckModule == null)
                {
                    Core.Server.Instance.Storages.PictureFrame.RemoveImage(constructionId);
                }
            }
        }

        /**
         *
         * Nesneyi bir yer'den başka bir yere de taşır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryPickupItem(WorldPickupItem pickupItem, Metadata.StorageContainer targetContainer, Metadata.StorageContainer sourceContainer = null, bool checkTargetContainer = true)
        {
            if (checkTargetContainer)
            {
                if (targetContainer == null || !targetContainer.HasRoomFor(pickupItem.Item) || targetContainer.IsItemExists(pickupItem.GetItemId()))
                {
                    return false;
                }
            }

            if (pickupItem.Source == PickupSourceType.CosmeticItem)
            {
                if (Core.Server.Instance.Storages.World.RemoveCosmeticItem(pickupItem.Item.ItemId))
                {
                    targetContainer?.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.Dynamic)
            {
                if (Core.Server.Instance.Storages.World.RemoveDynamicEntity(pickupItem.Item.ItemId))
                {
                    targetContainer?.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.EntitySlot)
            {
                if (Core.Server.Instance.Storages.World.DisableSlot(pickupItem.Item.ItemId))
                {
                    pickupItem.GenerateCustomId();
                    pickupItem.NextRespawnTime = Core.Server.Instance.Storages.World.GetSlotNextRespawnTime(pickupItem.Item.ItemId);

                    targetContainer?.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.Static)
            {
                if (Core.Server.Instance.Storages.World.AddDisablePersistentEntity(pickupItem.Item.ItemId))
                {
                    pickupItem.GenerateCustomId();

                    Log.Info("STATIC ADD P: " + pickupItem.Item.ItemId + ", C2: " + pickupItem.CustomUniqueId);

                    targetContainer?.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.StorageContainer)
            {
                if (sourceContainer == null || sourceContainer.RemoveItem(pickupItem.Item.ItemId))
                {
                    targetContainer.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.PlayerInventory)
            {
                if (sourceContainer.RemoveItem(pickupItem.Item.ItemId))
                {
                    targetContainer.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.NoSource)
            {
                if (sourceContainer == null)
                {
                    targetContainer.AddItem(pickupItem.GetStorageItem());
                    return true;
                }
            }
            else if (pickupItem.Source == PickupSourceType.PlayerInventoryDrop)
            {
                if (sourceContainer == null || !sourceContainer.IsItemExists(pickupItem.Item.ItemId))
                {
                    return false;
                }

                if (Core.Server.Instance.Storages.World.GetDynamicEntity(pickupItem.Item.ItemId) != null)
                {
                    return false;
                }

                sourceContainer.RemoveItem(pickupItem.Item);
                return true;
            }

            return false;
        }

        /**
         *
         * Verilen kaynak türüne göre nesneyi dünya üzerinden alır ve yerine başka nesne koyar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryPickupToWorld(WorldPickupItem pickupItem, out WorldDynamicEntity entity)
        {
            return TryPickupToWorld(pickupItem, null, out entity);
        }

        /**
         *
         * Verilen kaynak türüne göre nesneyi dünya üzerinden alır ve yerine başka nesne koyar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryPickupToWorld(WorldPickupItem pickupItem, Metadata.StorageContainer sourceContainer, out WorldDynamicEntity entity)
        {
            if (Core.Server.Instance.Storages.World.GetDynamicEntity(pickupItem.GetItemId()) != null)
            {
                entity = null;
                return false;
            }

            if (this.TryPickupItem(pickupItem, sourceContainer: sourceContainer, targetContainer: null, checkTargetContainer: false))
            {
                var dynamicEntity = Core.Server.Instance.Logices.World.CreateDynamicEntity(pickupItem.GetItemId(), pickupItem.Item.Item, pickupItem.Item.TechType, null, null, isDeployed: false);
                if (dynamicEntity == null)
                {
                    entity = null;
                    return false;
                }

                entity = dynamicEntity;
                return true;
            }

            entity = null;
            return false;
        }
    }
}

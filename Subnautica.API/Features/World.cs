namespace Subnautica.API.Features
{
    using System;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    using Subnautica.Network.Models.WorldEntity;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    public class World
    {
        /**
         *
         * BuilderTool.Start - > Yapı tamamlanma sesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static FMODAsset ConstructionCompleteSound { get; set; } = new FMODAsset()
        {
            name = "complete",
            path = "event:/tools/builder/complete",
            id   = "{b05ad9e7-17b0-4d61-b541-4bbf941cd7a5}",
        };

        /**
         *
         * BuilderTool.Start - > Yapı yıkılma sesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static FMODAsset DeconstructCompleteSound { get; set; } = new FMODAsset()
        {
            name = "deconstruct",
            path = "event:/bz/tools/build_tool/deconstruct",
            id   = "{bf237309-159c-48f0-a628-9926aa2a4181}",
        };

        /**
         *
         * Dünyanın yüklenip yüklenmediği.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsLoaded { get; set; } = false;

        /**
         *
         * Dünyanın yüklenip yüklenmediği.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Func<string, bool> OnGameObjectDestroyingAction { get; set; }

        /**
         *
         * Dünyanın yüklenip yüklenmediğini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetLoaded(bool isLoaded)
        {
            IsLoaded = isLoaded;
        }

        /**
         *
         * Nesne'yi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DestroyGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            var uniqueId = Network.Identifier.GetIdentityId(gameObject, false);

            if (OnGameObjectDestroyingAction == null || uniqueId.IsNull())
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                if (OnGameObjectDestroyingAction.Invoke(uniqueId))
                {
                    UnityEngine.GameObject.Destroy(gameObject);
                }
            }

            return true;
        }

        /**
         *
         * Nesne'yi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DestroyItem(string itemId)
        {
            if (Network.DynamicEntity.HasEntity(itemId))
            {
                Network.DynamicEntity.Remove(itemId);
            }
            else if (Network.StaticEntity.IsStaticEntity(itemId))
            {
                var entity = Network.StaticEntity.GetEntity(itemId);
                if (entity == null)
                {
                    entity = new StaticEntity()
                    {
                        UniqueId = itemId,
                    };
                }

                entity.DisableSpawn();

                Entity.RemoveToQueue(itemId);
            }
        }

        /**
         *
         * Nesne'yi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DestroyPickupItem(WorldPickupItem pickupItem)
        {
            if (pickupItem.Source == Enums.PickupSourceType.Dynamic)
            {
                Network.DynamicEntity.Remove(pickupItem.Item.ItemId);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.Static)
            {
                var entity = Network.StaticEntity.GetEntity(pickupItem.Item.ItemId);
                if (entity == null)
                {
                    entity = new StaticEntity() { UniqueId = pickupItem.Item.ItemId };
                }

                entity.DisableSpawn();

                Entity.RemoveToQueue(pickupItem.Item.ItemId);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.EntitySlot)
            {
                Network.WorldStreamer.DisableSlot(pickupItem.Item.ItemId.WorldStreamerToSlotId(), pickupItem.NextRespawnTime);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.PlayerInventory)
            {
                Entity.RemoveToQueue(pickupItem.Item.ItemId);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.CosmeticItem)
            {
                Network.Session.RemoveCosmeticItem(pickupItem.Item.ItemId);

                Entity.RemoveToQueue(pickupItem.Item.ItemId);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.StorageContainer)
            {
                Entity.RemoveToQueue(pickupItem.Item.ItemId);
            }
            else if (pickupItem.Source == Enums.PickupSourceType.NoSource)
            {
                Entity.RemoveToQueue(pickupItem.Item.ItemId);
            }
        }

        /**
         *
         * Alınan bir eşyayı spawnlar ve yerine yerleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnPickupItem(WorldPickupItem worldPickupItem, ItemsContainer container, ItemQueueAction item = null)
        {
            if (worldPickupItem.Item.Item == null)
            {
                Entity.SpawnToQueue(worldPickupItem.Item.TechType, worldPickupItem.GetItemId(), container, item); 
            }
            else
            {
                Entity.SpawnToQueue(worldPickupItem.Item.Item, worldPickupItem.GetItemId(), container, item); 
            }
        }

        /**
         *
         * Bir eşyayı spawnlar ve oyuncu envanterine yerleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnPickupItemToInventory(WorldPickupItem worldPickupItem, ItemQueueAction item = null)
        {
            if (worldPickupItem.Item.Item == null)
            {
                Entity.SpawnToQueue(worldPickupItem.Item.TechType, worldPickupItem.GetItemId(), global::Inventory.Get().container, item); 
            }
            else
            {
                Entity.SpawnToQueue(worldPickupItem.Item.Item, worldPickupItem.GetItemId(), global::Inventory.Get().container, item); 
            }
        }

        /**
         *
         * Nesne'yi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DestroyItemFromPlayer(TechType techType)
        {
            var pickupable = global::Inventory.main.container.RemoveItem(techType);
            if (pickupable != null && pickupable.gameObject != null)
            {
                DestroyGameObject(pickupable.gameObject);
            }
        }

        /**
         *
         * Nesne'yi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DestroyItemFromPlayer(string uniqueId, bool resetArm = false)
        {
            var inventoryItem = Network.Identifier.GetGameObject(uniqueId);
            if (inventoryItem)
            {
                if (inventoryItem.TryGetComponent<global::Pickupable>(out var pickupable))
                {
                    pickupable.SetVisible(false);
                    pickupable.Reparent(null);
                    pickupable.Activate(false);
                    pickupable.attached = false;
                }

                inventoryItem.SetActive(false);

                if (resetArm)
                {
                    global::Player.main.armsController.Reconfigure(null);
                }

                DestroyGameObject(inventoryItem);
            }
        }

        /**
         *
         * Tüm verileri siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Dispose()
        {
            World.OnGameObjectDestroyingAction = null;
            PlayerCinematicController.cinematicModeCount = 0;
        }
    }
}

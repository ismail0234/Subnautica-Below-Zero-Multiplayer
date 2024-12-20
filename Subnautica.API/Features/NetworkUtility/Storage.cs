namespace Subnautica.API.Features.NetworkUtility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using UWE;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class Storage
    {
        /**
         *
         * İşlem Kuyruğunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private readonly List<StorageProcessItem> Queue = new List<StorageProcessItem>();
        
        /**
         *
         * Bekleme zamanı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private readonly WaitForSecondsRealtime WaitTime = new WaitForSecondsRealtime(0.25f);

        /**
         *
         * Kuyruğun tüketilip/tüketilmediği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsRunning { get; set; } = false;

        /**
         *
         * Gecikme süresi (second)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double MaxDelay = 2.5;

        /**
         *
         * Envantere nesne ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InitializeStorage(string containerId, Metadata.StorageContainer storageContainer, Action<ItemQueueProcess, Pickupable, GameObject> onEntitySpawned = null, object customProperty = null)
        {
            if (storageContainer != null)
            {
                var container = this.GetItemContainer(containerId);
                if (container != null)
                {
                    var action = new ItemQueueAction();
                    action.OnEntitySpawned = onEntitySpawned;
                    action.RegisterProperty("CustomProperty", customProperty);

                    foreach (var item in storageContainer.Items)
                    {
                        Entity.SpawnToQueue(item.Item, item.ItemId, container, action);
                    }
                }
            }
        }

        /**
         *
         * Depoyaya nesne ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddItemToStorage(string containerId, byte playerId, WorldPickupItem worldPickupItem, Action<ItemQueueProcess, Pickupable, GameObject> onEntitySpawned = null, object customProperty = null)
        {
            this.Queue.Add(new StorageProcessItem()
            {
                OwnerId         = playerId,
                ContainerId     = containerId,
                WorldPickupItem = worldPickupItem,
                OnEntitySpawned = onEntitySpawned,
                CustomProperty  = customProperty
            });

            this.ConsumeQueue();
        }

        /**
         *
         * Envantere nesne ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddItemToInventory(byte playerId, WorldPickupItem pickupItem, Action<ItemQueueProcess, Pickupable, GameObject> onEntitySpawned = null, object customProperty = null, bool showNotify = false)
        {
            World.DestroyPickupItem(pickupItem);

            if (ZeroPlayer.IsPlayerMine(playerId))
            {
                if (showNotify)
                {
                    pickupItem.Item.TechType.ShowPickupNotify();
                }

                if (onEntitySpawned == null)
                {
                    World.SpawnPickupItemToInventory(pickupItem);
                }
                else
                {
                    var action = new ItemQueueAction();
                    action.OnEntitySpawned = onEntitySpawned;
                    action.RegisterProperty("CustomProperty", customProperty);

                    World.SpawnPickupItemToInventory(pickupItem, action);
                }
            }
        }

        /**
         *
         * Kuyruktaki nesneleri tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ConsumeQueue()
        {
            if (this.IsRunning)
            {
                return false;
            }

            CoroutineHost.StartCoroutine(this.ConsumeQueueAsync());
            return true;
        }

        /**
         *
         * Kuyruktaki nesneleri tüketir. (Async)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator ConsumeQueueAsync()
        {
            this.IsRunning = true;

            while (this.Queue.Count > 0)
            {
                foreach (var item in this.Queue.ToList())
                {
                    if (item.IsFirstTime())
                    {
                        item.FirstCheckTime = Network.Session.GetWorldTime();

                        if (this.AddItemToStorage(item, true))
                        {
                            this.Queue.Remove(item);
                            continue;
                        }
                    }

                    if (item.IsDelayed())
                    {
                        Log.Error("ITEM VERY DELAYED. MAYBE DE-SYNC...");
                        this.Queue.Remove(item);
                    }
                    else 
                    {
                        if (this.AddItemToStorage(item, false))
                        {
                            this.Queue.Remove(item);
                        }
                    }
                }

                if (this.Queue.Count > 0)
                {
                    yield return this.WaitTime;
                }
            }

            this.IsRunning = false;
        }

        /**
         *
         * Depoya eşya ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool AddItemToStorage(StorageProcessItem item, bool destroyItem)
        {
            if (destroyItem && ZeroPlayer.IsPlayerMine(item.OwnerId))
            {
                Entity.RemoveToQueue(item.WorldPickupItem.Item.ItemId);
            }

            var container = this.GetItemContainer(item.ContainerId);
            if (container == null)
            {
                return false;
            }

            var action = new ItemQueueAction();
            action.OnEntitySpawned = item.OnEntitySpawned;
            action.RegisterProperty("CustomProperty", item.CustomProperty);

            World.SpawnPickupItem(item.WorldPickupItem, container, action);
            return true;
        }

        /**
         *
         * Nesne kapsayıcısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ItemsContainer GetItemContainer(string containerId)
        {
            var gameObject = Network.Identifier.GetGameObject(containerId, true);
            if (gameObject == null)
            {
                return null;
            }

            if (gameObject.TryGetComponent<global::BaseBioReactorGeometry>(out var bioReactor))
            {
                if (bioReactor.GetModule())
                {
                    return bioReactor.GetModule().container;
                }
            }
            else if (gameObject.TryGetComponent<global::StorageContainer>(out var storageContainer))
            {
                return storageContainer.container;
            }
            else if (gameObject.TryGetComponent<global::BaseDeconstructable>(out var baseDeconstructable))
            {
                return baseDeconstructable.GetMapRoomFunctionality()?.storageContainer?.container;
            }
            else 
            {
                var storage = gameObject.GetComponentInChildren<global::StorageContainer>();
                if (storage)
                {
                    return storage.container;    
                }
            }

            return null;
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.IsRunning = false;
            this.Queue.Clear();
        }
    }

    public class StorageProcessItem
    {
        /**
         *
         * OwnerId nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte OwnerId { get; set; }

        /**
         *
         * ContainerId nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ContainerId { get; set; }

        /**
         *
         * FirstCheckTime nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double FirstCheckTime { get; set; } = 0f;

        /**
         *
         * WorldPickupItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldPickupItem WorldPickupItem { get; set; }

        /**
         *
         * ProcessCompleted nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Action<ItemQueueProcess, Pickupable, GameObject> OnEntitySpawned { get; set; }

        /**
         *
         * CustomProperty nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public object CustomProperty { get; set; }

        /**
         *
         * İlk sefer mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFirstTime()
        {
            return this.FirstCheckTime == 0f;
        }

        /**
         *
         * Gecikti mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDelayed()
        {
            return Network.Session.GetWorldTime() > this.FirstCheckTime + Network.Storage.MaxDelay;
        }
    }
}
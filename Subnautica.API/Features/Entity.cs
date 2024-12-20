namespace Subnautica.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using UWE;

    public class Entity
    {
        /**
         *
         * Kuyruğu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Queue<ItemQueueProcess> Queue { get; set; } = new Queue<ItemQueueProcess>();

        /**
         *
         * Çerçeve Başına İşlem Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static byte ConsumptionPerFrame { get; set; } = 8;

        /**
         *
         * Mevcut çerçevede yapılan işlem sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static byte CurrentConsumptionCount { get; set; } = 0;

        /**
         *
         * Kuyruğun tüketilip/tüketilmediği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsRunning { get; private set; } = false;

        /**
         *
         * Kuyruktaki nesne sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int QueueTotalCount()
        {
            return Queue.Count;
        }

        /**
         *
         * (İşlem) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ProcessToQueue(ItemQueueAction action)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsProcess = true,
                Action    = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(TechType techType, ItemsContainer container, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                TechType   = techType, 
                Container  = container,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(TechType techType, string itemId, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                ItemId     = itemId,
                TechType   = techType,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(TechType techType, string itemId, ItemsContainer container, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                ItemId     = itemId,
                TechType   = techType,
                Container  = container,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(byte[] item, string itemId, ZeroTransform transform, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                ItemId     = itemId,
                Item       = item,
                Transform  = transform,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(TechType techType, string itemId, ZeroTransform transform, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                TechType   = techType,
                ItemId     = itemId,
                Transform  = transform,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(byte[] item, string itemId, ItemsContainer container, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                ItemId     = itemId, 
                Item       = item, 
                Container  = container,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Ekipman) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(string slotId, TechType techType, Equipment equipment, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                SlotId     = slotId, 
                TechType   = techType, 
                Equipment  = equipment,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Depolama) Kaldırma/Yoketme kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveToQueue(Pickupable pickupable, ItemsContainer container, ItemQueueAction action = null)
        {            
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = false,
                Pickupable = pickupable, 
                Container  = container,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Ekipman) Yumurtlama kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnToQueue(string slotId, TechType techType, string itemId, Equipment equipment, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = true,
                ItemId     = itemId,
                SlotId     = slotId,
                TechType   = techType,
                Equipment  = equipment,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Ekipman) Kaldırma/Yoketme kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveToQueue(string slotId, Equipment equipment, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = false,
                SlotId     = slotId, 
                Equipment  = equipment,
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * (Nesne) Kaldırma/Yoketme kuyruğuna ekleme yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveToQueue(string itemId, ItemQueueAction action = null)
        {
            Entity.Queue.Enqueue(new ItemQueueProcess()
            {
                IsSpawning = false,
                ItemId     = itemId, 
                Action     = action,
            });

            Entity.ConsumeQueue();
        }

        /**
         *
         * Kuyruktaki nesneleri tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool ConsumeQueue()
        {
            if (Entity.IsRunning)
            {
                return false;
            }

            CoroutineHost.StartCoroutine(Entity.ConsumeQueueAsync());
            return true;
        }

        /**
         *
         * Kuyruktaki nesneleri tüketir. (Async)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator ConsumeQueueAsync()
        {
            Entity.IsRunning = true;
            Entity.ResetConsumption();

            while (Entity.Queue.Count > 0)
            {
                if (Entity.CurrentConsumptionCount > Entity.ConsumptionPerFrame)
                {
                    Entity.ResetConsumption();
                    yield return CoroutineUtils.waitForNextFrame;
                }

                var item = Entity.Queue.Dequeue();
                if (item.IsProcess)
                {
                    if (item.Action.OnProcessCompletedAsync != null)
                    {
                        yield return item.Action.OnProcessCompletedAsync(item);
                    }
                    else if (item.Action.OnProcessCompleted != null)
                    {
                        item.Action.OnProcessCompleted(item);    
                    }
                    
                    continue;
                }

                if (item.IsSpawning)
                {
                    if (item.Action != null && item.Action.OnEntitySpawning != null)
                    {
                        if (!item.Action.OnEntitySpawning(item))
                        {
                            continue;
                        }
                    }

                    GameObject itemGameObject = null;
                    if (item.Item != null)
                    {
                        using (PooledObject<ProtobufSerializer> proxy = ProtobufSerializerPool.GetProxy())
                        {
                            using (MemoryStream stream = new MemoryStream(item.Item))
                            {
                                var coroutineTask = proxy.Value.DeserializeObjectTreeAsync(stream, false, false, 0);

                                yield return coroutineTask;

                                itemGameObject = coroutineTask.GetResult();

                                coroutineTask = null;
                            }
                        }
                    }
                    else if (item.TechType != TechType.None)
                    {
                        var coroutineTask = new TaskResult<GameObject>();

                        yield return CraftData.InstantiateFromPrefabAsync(item.TechType, coroutineTask);

                        itemGameObject = coroutineTask.Get();
                    }

                    if (itemGameObject == null)
                    {
                        continue;
                    }

                    if (item.ItemId != null)
                    {
                        Network.Identifier.SetIdentityId(itemGameObject, item.ItemId);
                    }


                    Entity.CurrentConsumptionCount++;

                    try
                    {
                        var pickupable = itemGameObject.GetComponent<Pickupable>();
                        if (pickupable == null)
                        {
                            if (item.Transform != null)
                            {
                                itemGameObject.transform.position = item.Transform.Position.ToVector3();
                                itemGameObject.transform.rotation = item.Transform.Rotation.ToQuaternion();
                            }

                            if (item.Action != null && item.Action.OnEntitySpawned != null)
                            {
                                item.Action.OnEntitySpawned(item, pickupable, itemGameObject);
                            }

                            continue;
                        }

                        pickupable.transform.parent = null;
                        pickupable.Initialize();

                        if (item.Container != null)
                        {
                            Entity.AddItemToContainer(item, itemGameObject, pickupable);
                        }
                        else if (item.Equipment != null)
                        {
                            Entity.AddItemToEquipment(item, itemGameObject, pickupable);
                        }
                        else if (item.Transform != null)
                        {
                            pickupable.transform.position = item.Transform.Position.ToVector3();
                            pickupable.transform.rotation = item.Transform.Rotation.ToQuaternion();
                            
                            if (item.Action != null && item.Action.OnEntitySpawned != null)
                            {
                                item.Action.OnEntitySpawned(item, pickupable, itemGameObject);
                            }
                        }
                        else
                        {
                            if (item.Action != null && item.Action.OnEntitySpawned != null)
                            {
                                item.Action.OnEntitySpawned(item, pickupable, itemGameObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Entity.Queue.Spawning Exception: {e}");
                    }
                }
                else
                {
                    Entity.CurrentConsumptionCount++;

                    try
                    {
                        if (item.Container != null)
                        {
                            Entity.RemoveItemInContainer(item);
                        }
                        else if (item.Equipment != null)
                        {
                            Entity.RemoveItemInEquipment(item);
                        }
                        else if (item.ItemId != null)
                        {
                            Entity.RemoveItemInWorld(item);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Entity.Queue.Removing Exception: {e}");
                    }
                }
            }

            Entity.IsRunning = false;
        }
        
        /**
         *
         * Ekipman'a nesneyi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void AddItemToEquipment(ItemQueueProcess item, GameObject itemGameObject, Pickupable pickupable)
        {
            if (item.Equipment != null)
            {
                Entity.RemoveItemInEquipment(item);

                using (EventBlocker.Create(item.TechType))
                {
                    if (item.Equipment.AddItem(item.SlotId, new InventoryItem(pickupable), true))
                    {
                        if (item.Action != null && item.Action.OnEntitySpawned != null)
                        {
                            item.Action.OnEntitySpawned(item, pickupable, itemGameObject);
                        }
                    }
                    else
                    {
                        World.DestroyGameObject(itemGameObject);
                    }
                }
            }
            else 
            {
                World.DestroyGameObject(itemGameObject);
            }
        }

        /**
         *
         * Depolamaya nesneyi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void AddItemToContainer(ItemQueueProcess item, GameObject itemGameObject, Pickupable pickupable)
        {
            if (item.Container != null)
            {
                using (EventBlocker.Create(pickupable.GetTechType()))
                using (EventBlocker.Create(TechType.Locker))
                using (EventBlocker.Create(TechType.SmallLocker))
                using (EventBlocker.Create(TechType.Fridge))
                using (EventBlocker.Create(TechType.PlanterPot))
                using (EventBlocker.Create(TechType.PlanterPot2))
                using (EventBlocker.Create(TechType.PlanterPot3))
                using (EventBlocker.Create(TechType.PlanterBox))
                using (EventBlocker.Create(TechType.PlanterShelf))
                using (EventBlocker.Create(TechType.FarmingTray))
                using (EventBlocker.Create(TechType.BaseWaterPark))
                using (EventBlocker.Create(TechType.EscapePod))
                using (EventBlocker.Create(TechType.Exosuit))
                using (EventBlocker.Create(TechType.SmallStorage))
                using (EventBlocker.Create(TechType.QuantumLocker))
                using (EventBlocker.Create(TechType.SeaTruckStorageModule))
                using (EventBlocker.Create(TechType.SeaTruckFabricatorModule))
                using (EventBlocker.Create(TechType.SeaTruckAquariumModule))
                using (EventBlocker.Create(TechType.SpyPenguin))
                using (EventBlocker.Create(ProcessType.InventoryItem))
                using (EventBlocker.Create(ProcessType.InventoryQuickSlot))
                {
                    item.Container.UnsafeAdd(new InventoryItem(pickupable));
                }

                item.Container.Sort();

                if (item.Action != null && item.Action.OnEntitySpawned != null)
                {
                    item.Action.OnEntitySpawned(item, pickupable, itemGameObject);
                }
            }
            else 
            {
                World.DestroyGameObject(itemGameObject);
            }
        }

        /**
         *
         * Eşyayı yok eder ve ekipmanlardan kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool RemoveItemInEquipment(ItemQueueProcess item)
        {
            if (string.IsNullOrEmpty(item.SlotId) || item.Equipment == null)
            {
                return false;
            }

            if (item.Equipment.equipment.TryGetValue(item.SlotId, out var equipmentItem) && equipmentItem != null)
            {
                using (EventBlocker.Create(equipmentItem.item.GetTechType()))
                {
                    var inventoryItem = item.Equipment.RemoveItem(item.SlotId, true, false);
                    if (inventoryItem != null && inventoryItem.item != null)
                    {
                        World.DestroyGameObject(inventoryItem.item.gameObject);
                    }
                }
            }

            if (item.Action != null && item.Action.OnEntityRemoved != null)
            {
                item.Action.OnEntityRemoved(item);
            }
            
            return true;
        }

        /**
         *
         * Eşyayı yok eder ve kapsayıcı'dan kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool RemoveItemInContainer(ItemQueueProcess item)
        {
            if (item.Container != null)
            {
                if (item.Pickupable != null)
                {
                    using (EventBlocker.Create(item.Pickupable.GetTechType()))
                    using (EventBlocker.Create(TechType.Locker))
                    using (EventBlocker.Create(TechType.SmallLocker))
                    using (EventBlocker.Create(TechType.Fridge))
                    using (EventBlocker.Create(TechType.PlanterPot))
                    using (EventBlocker.Create(TechType.PlanterPot2))
                    using (EventBlocker.Create(TechType.PlanterPot3))
                    using (EventBlocker.Create(TechType.PlanterBox))
                    using (EventBlocker.Create(TechType.PlanterShelf))
                    using (EventBlocker.Create(TechType.FarmingTray))
                    using (EventBlocker.Create(TechType.BaseWaterPark))
                    using (EventBlocker.Create(TechType.EscapePod))
                    using (EventBlocker.Create(TechType.Exosuit))
                    using (EventBlocker.Create(TechType.SmallStorage))
                    using (EventBlocker.Create(TechType.QuantumLocker))
                    using (EventBlocker.Create(TechType.SeaTruckStorageModule))
                    using (EventBlocker.Create(TechType.SeaTruckFabricatorModule))
                    using (EventBlocker.Create(TechType.SeaTruckAquariumModule))
                    using (EventBlocker.Create(TechType.SpyPenguin))
                    using (EventBlocker.Create(ProcessType.InventoryItem))
                    using (EventBlocker.Create(ProcessType.InventoryQuickSlot))
                    {
                        item.Container.RemoveItem(item.Pickupable, true);
                        item.Container.Sort();
                    }
                }
            }

            if (item.Pickupable != null && item.Pickupable.gameObject != null)
            {
                World.DestroyGameObject(item.Pickupable.gameObject);
            }

            if (item.Action != null && item.Action.OnEntityRemoved != null)
            {
                item.Action.OnEntityRemoved(item);
            }

            return true;
        }

        /**
         *
         * Dünya'dan nesneyi kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void RemoveItemInWorld(ItemQueueProcess item)
        {
            var pickupable = Network.Identifier.GetComponentByGameObject<global::Pickupable>(item.ItemId, true);
            if (pickupable)
            {
                if (pickupable.inventoryItem != null && pickupable.inventoryItem.container != null)
                {
                    using (EventBlocker.Create(pickupable.GetTechType()))
                    using (EventBlocker.Create(TechType.Locker))
                    using (EventBlocker.Create(TechType.SmallLocker))
                    using (EventBlocker.Create(TechType.Fridge))
                    using (EventBlocker.Create(TechType.PlanterPot))
                    using (EventBlocker.Create(TechType.PlanterPot2))
                    using (EventBlocker.Create(TechType.PlanterPot3))
                    using (EventBlocker.Create(TechType.PlanterBox))
                    using (EventBlocker.Create(TechType.PlanterShelf))
                    using (EventBlocker.Create(TechType.FarmingTray))
                    using (EventBlocker.Create(TechType.BaseWaterPark))
                    using (EventBlocker.Create(TechType.EscapePod))
                    using (EventBlocker.Create(TechType.Exosuit))
                    using (EventBlocker.Create(TechType.SmallStorage))
                    using (EventBlocker.Create(TechType.QuantumLocker))
                    using (EventBlocker.Create(TechType.SeaTruckStorageModule))
                    using (EventBlocker.Create(TechType.SeaTruckFabricatorModule))
                    using (EventBlocker.Create(TechType.SeaTruckAquariumModule))
                    using (EventBlocker.Create(TechType.SpyPenguin))
                    using (EventBlocker.Create(ProcessType.InventoryItem))
                    using (EventBlocker.Create(ProcessType.InventoryQuickSlot))
                    {
                        pickupable.inventoryItem.container.RemoveItem(pickupable.inventoryItem, true, false);
                    }
                }

                World.DestroyGameObject(pickupable.gameObject);
            }
            else
            {
                var gameObject = Network.Identifier.GetGameObject(item.ItemId, true);
                if (gameObject)
                {
                    World.DestroyGameObject(gameObject);
                }
            }

            if (item.Action != null && item.Action.OnEntityRemoved != null)
            {
                item.Action.OnEntityRemoved(item);
            }
        }

        /**
         *
         * Mevcut çerçevede yapılan işlem sayısını sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void ResetConsumption()
        {
            Entity.CurrentConsumptionCount = 0;
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
            Queue.Clear();

            Entity.IsRunning = false;
        }
    }
}

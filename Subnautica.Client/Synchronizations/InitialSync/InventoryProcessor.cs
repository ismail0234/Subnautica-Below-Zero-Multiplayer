namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    using UnityEngine;

    using UWE;

    public class InventoryProcessor
    {
        /**
         *
         * Envanter verilerini yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInventoryInitialized()
        {
            if (Network.Session.Current.PlayerInventoryItems?.Items?.Count > 0)
            {
                foreach (var item in Network.Session.Current.PlayerInventoryItems.Items)
                {
                    if (item.Item == null)
                    {
                        Entity.SpawnToQueue(item.TechType, item.ItemId, global::Inventory.main.container);
                    }
                    else
                    {
                        Entity.SpawnToQueue(item.Item, item.ItemId, global::Inventory.main.container);
                    }
                }
            }
        }

        /**
         *
         * Ekipman verilerini yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator OnEquipmentInitialized()
        {
            if (Network.Session.Current.PlayerEquipments != null && Network.Session.Current.PlayerEquipments.Length > 0)
            {
                using (PooledObject<ProtobufSerializer> proxy = ProtobufSerializerPool.GetProxy())
                {
                    using (EventBlocker.Create(ProcessType.InventoryEquipment))
                    {
                        yield return RestoreEquipmentAsync(proxy.Value, Network.Session.Current.PlayerEquipments, Network.Session.Current.PlayerEquipmentSlots, global::Inventory.main.equipment);
                    }
                }
            }
        }

        /**
         *
         * Envanter Hızlı slotları ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInventoryQuickSlotsInitialized()
        {
            if (Network.Session.Current.PlayerQuickSlots != null)
            {
                using (EventBlocker.Create(ProcessType.InventoryQuickSlot))
                {
                    global::Inventory.main.quickSlots.RestoreBinding(Network.Session.Current.PlayerQuickSlots);

                    if (Network.Session.Current.PlayerActiveSlot > -1)
                    {
                        global::Inventory.main.quickSlots.SelectImmediate(Network.Session.Current.PlayerActiveSlot);
                    }
                    else
                    {
                        global::Inventory.main.quickSlots.DeselectImmediate();
                    }
                }
            }
        }

        /**
         *
         * Ekipman verilerini yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator RestoreEquipmentAsync(ProtobufSerializer serializer, byte[] serialItems, Dictionary<string, string> serialSlots, Equipment equipment)
        {
            using (MemoryStream stream = new MemoryStream(serialItems))
            {
                var task = serializer.DeserializeObjectTreeAsync(stream, forceInactiveRoot: false, allowSpawnRestrictions: false, 0);
                yield return task;

                var dictionary = new Dictionary<string, InventoryItem>();
                var result     = task.GetResult();
                if (result != null)
                {
                    foreach (var uniqueIdentifier in result.GetComponentsInChildren<UniqueIdentifier>(includeInactive: true))
                    {
                        if (uniqueIdentifier.transform.parent == result.transform)
                        {
                            dictionary[uniqueIdentifier.Id] = new InventoryItem(uniqueIdentifier.gameObject.EnsureComponent<Pickupable>());
                        }
                    }

                    equipment.RestoreEquipment(serialSlots, dictionary);

                    Object.Destroy(result);
                }
            }
        }
    }
}

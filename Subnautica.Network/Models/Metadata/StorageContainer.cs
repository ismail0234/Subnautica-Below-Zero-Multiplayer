namespace Subnautica.Network.Models.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class StorageContainer : MetadataComponent
    {

        /**
         *
         * Eşyaları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public List<StorageItem> Items { get; set; } = new List<StorageItem>();

        /**
         *
         * Depo üzerindeki metni barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public Sign Sign { get; set; }

        /**
         *
         * Depo Boyutunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public byte Size { get; set; }

        /**
         *
         * Size X değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetSizeX()
        {
            return (byte)(this.Size % 10);
        }

        /**
         *
         * Size Y değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetSizeY()
        {
            return (byte)(this.Size / 10);
        }

        /**
         *
         * Depoyu yeniden boyutlandırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Resize(byte width, byte heigth)
        {
            this.Size = Convert.ToByte(width + (heigth * 10));

            if (this.ItemsMap != null)
            {
                this.ItemsMap = new InventoryItem[this.GetSizeX(), this.GetSizeY()];
            }
        }

        /**
         *
         * Eşyayı ekler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddItem(StorageItem item)
        {
            this.RemoveItem(item);
            this.Items.Add(item);
            return true;
        }

        /**
         *
         * Eşyayı siler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveItem(StorageItem item)
        {
            return this.RemoveItem(item.ItemId);
        }

        /**
         *
         * Eşyayı siler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveItem(string itemId)
        {
            return this.Items.RemoveAll(q => q.ItemId == itemId) > 0;
        }

        /**
         *
         * Envanterde nesne mevcut mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsItemExists(string uniqueId)
        {
            return this.Items.Any(q => q.ItemId == uniqueId);
        }

        /**
         *
         * Envanterde nesne türünden kaç adet olduğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetCount(TechType techType)
        {
            return this.Items.Count(q => q.TechType == techType);
        }

        /**
         *
         * Sınıfı oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StorageContainer Create(byte width, byte heigth)
        {
            var storageContainer = new StorageContainer();
            storageContainer.Resize(width, heigth);
            return storageContainer;
        }

        /**
         *
         * Nesne eklenebilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasRoomFor(StorageItem storageItem)
        {
            if (this.Items.Any(q => q.ItemId == storageItem.ItemId))
            {
                return false;
            }

            if (this.ItemsMap == null)
            {
                this.ItemsMap = new InventoryItem[this.GetSizeX(), this.GetSizeY()];
            }

            foreach (var item in this.Items)
            {
                if (!this.AllItemGroups.ContainsKey(item.TechType))
                {
                    this.AllItemGroups.Add(item.TechType, new ItemsContainer.ItemGroup((int)item.TechType, item.GetSizeX(), item.GetSizeY()));
                }

                if (this.AllItemGroups.TryGetValue(item.TechType, out var itemGroup))
                {
                    itemGroup.items.Add(new InventoryItem(item.GetSizeX(), item.GetSizeY()));
                }
            }

            foreach (var item in this.AllItemGroups)
            {
                this.ItemGroups.Add(item.Value);
            }

            this.GhostItem.SetGhostDims(storageItem.GetSizeX(), storageItem.GetSizeY());
            this.GhostGroup.SetGhostDims(storageItem.GetSizeX(), storageItem.GetSizeY());
            this.GhostGroup.items.Add(this.GhostItem);
            this.ItemGroups.Add(this.GhostGroup);

            var response = this.TrySort(this.ItemGroups, this.ItemsMap);

            ItemsContainer.ResetItemsMap(this.ItemsMap);
            this.GhostGroup.items.Clear();
            this.ItemGroups.Clear();
            this.AllItemGroups.Clear();

            return response;
        }

        /**
         *
         * Envanteri sıralar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool TrySort(List<ItemsContainer.ItemGroup> gr, InventoryItem[,] map)
        {
            var flag = true;
            ItemsContainer.ResetItemsMap(map);
            gr.Sort(ItemsContainer.groupComparer);

            for (int index1 = 0; index1 < gr.Count; ++index1)
            {
                var itemGroup = gr[index1];
                var width     = itemGroup.width;
                var height    = itemGroup.height;

                List<InventoryItem> items = itemGroup.items;
                for (int index2 = 0; index2 < items.Count; ++index2)
                {
                    var inventoryItem = items[index2];
                    if (!inventoryItem.ignoreForSorting)
                    {
                        if (ItemsContainer.GetRoomFor(map, width, height, out var x, out var y))
                        {
                            ItemsContainer.AddItemToMap(map, x, y, inventoryItem);
                            inventoryItem.SetPosition(x, y);
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    break;
                }
            }

            return flag;
        }

        /**
         *
         * ItemGroups nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private Dictionary<TechType, ItemsContainer.ItemGroup> AllItemGroups { get; set; } = new Dictionary<TechType, ItemsContainer.ItemGroup>();

        /**
         *
         * GhostItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private InventoryItem GhostItem { get; set; } = new InventoryItem(1, 1);

        /**
         *
         * GhostGroup nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private ItemsContainer.ItemGroup GhostGroup { get; set; } = new ItemsContainer.ItemGroup(0, 1, 1);

        /**
         *
         * GhostGroup nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public List<ItemsContainer.ItemGroup> ItemGroups { get; set; } = new List<ItemsContainer.ItemGroup>();

        /**
         *
         * Harita listesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private InventoryItem[,] ItemsMap { get; set; } = null;
    }
}
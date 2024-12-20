namespace Subnautica.Events.EventArgs
{
    using System;

    public class StorageItemAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StorageItemAddedEventArgs(string constructionId, TechType techType, string itemId, Pickupable item, InventoryItem inventoryItem)
        {
            this.ConstructionId = constructionId;
            this.TechType       = techType;
            this.ItemId         = itemId;
            this.Item           = item;
            this.InventoryItem  = inventoryItem;
        }

        /**
         *
         * ConstructionId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ConstructionId { get; set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * ItemId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ItemId { get; set; }

        /**
         *
         * Item Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Item { get; set; }

        /**
         *
         * InventoryItem Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public InventoryItem InventoryItem { get; set; }
    }
}

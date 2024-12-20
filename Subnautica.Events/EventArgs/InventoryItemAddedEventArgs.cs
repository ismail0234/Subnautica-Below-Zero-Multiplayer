namespace Subnautica.Events.EventArgs
{
    using System;

    public class InventoryItemAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public InventoryItemAddedEventArgs(string uniqueId, Pickupable item)
        {
            this.UniqueId = uniqueId;
            this.Item     = item;
        }

        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Item Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Item { get; set; }
    }
}

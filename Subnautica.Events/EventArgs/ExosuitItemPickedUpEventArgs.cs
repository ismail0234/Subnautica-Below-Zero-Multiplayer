namespace Subnautica.Events.EventArgs
{
    using System;
    
    using Subnautica.API.Features;

    public class ExosuitItemPickedUpEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ExosuitItemPickedUpEventArgs(string uniqueId, string itemId, Pickupable item, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.ItemId    = itemId;
            this.Item      = item;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * ItemId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ItemId { get; private set; }

        /**
         *
         * Item Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Item { get; private set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
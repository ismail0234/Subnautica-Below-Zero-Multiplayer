namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlanterItemAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlanterItemAddedEventArgs(string uniqueId, string itemId, Plantable plantable, int slotId)
        {
            this.UniqueId  = uniqueId;
            this.ItemId    = itemId;
            this.Plantable = plantable;
            this.SlotId    = slotId;
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
         * Side Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ItemId { get; set; }

        /**
         *
         * Plantable Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Plantable Plantable { get; set; }

        /**
         *
         * SlotId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int SlotId { get; set; }
    }
}

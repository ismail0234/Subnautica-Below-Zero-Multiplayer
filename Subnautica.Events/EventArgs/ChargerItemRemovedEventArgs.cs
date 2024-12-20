namespace Subnautica.Events.EventArgs
{
    using System;

    public class ChargerItemRemovedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ChargerItemRemovedEventArgs(string constructionId, string slotId, TechType techType, string itemId, Pickupable item)
        {
            this.ConstructionId = constructionId;
            this.SlotId         = slotId;
            this.TechType       = techType;
            this.ItemId         = itemId;
            this.Item           = item;
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
         * SlotId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SlotId { get; set; }

        /**
         *
         * TechType değeri
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
    }
}

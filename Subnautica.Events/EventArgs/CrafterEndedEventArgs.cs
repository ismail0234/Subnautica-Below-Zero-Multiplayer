namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrafterEndedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrafterEndedEventArgs(string uniqueId, TechType crafterTechType, TechType techType, global::GhostCrafter crafter)
        {
            this.UniqueId        = uniqueId;
            this.CrafterTechType = crafterTechType;
            this.TechType        = techType;
            this.Crafter         = crafter;
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
         * CrafterTechType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType CrafterTechType { get; set; }

        /**
         *
         * TechType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * Crafter Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::GhostCrafter Crafter { get; set; }
    }
}

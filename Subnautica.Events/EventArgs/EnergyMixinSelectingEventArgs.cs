namespace Subnautica.Events.EventArgs
{
    using System;

    public class EnergyMixinSelectingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EnergyMixinSelectingEventArgs(string uniqueId, string batterySlotId, TechType batteryType, TechType techType, Pickupable item, bool isAdding = false, bool isChanging = false, bool isAllowed = true)
        {
            this.UniqueId      = uniqueId;
            this.BatterySlotId = batterySlotId;
            this.BatteryType   = batteryType;
            this.TechType      = techType;
            this.Item          = item;
            this.IsAdding      = isAdding;
            this.IsChanging    = isChanging;
            this.IsAllowed     = isAllowed;
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
         * BatterySlotId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string BatterySlotId { get; set; }

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
         * BatteryType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType BatteryType { get; set; }

        /**
         *
         * Item Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Item { get; set; }

        /**
         *
         * IsAdding Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAdding { get; set; }

        /**
         *
         * IsChanging Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsChanging { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

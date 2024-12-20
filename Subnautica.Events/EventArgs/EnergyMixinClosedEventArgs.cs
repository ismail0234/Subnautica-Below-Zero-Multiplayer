namespace Subnautica.Events.EventArgs
{
    using System;

    public class EnergyMixinClosedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EnergyMixinClosedEventArgs(string uniqueId, string batterySlotId, TechType techType)
        {
            this.UniqueId      = uniqueId;
            this.BatterySlotId = batterySlotId;
            this.TechType      = techType;
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
    }
}

namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    public class EnergyMixinClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EnergyMixinClickingEventArgs(string uniqueId, string batterySlotId, TechType techType, bool isAllowed = true)
        {
            this.UniqueId      = uniqueId;
            this.BatterySlotId = batterySlotId.Replace(ZeroGame.GetVehicleBatteryLabelUniqueId(null, true), "");
            this.TechType      = techType;
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
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

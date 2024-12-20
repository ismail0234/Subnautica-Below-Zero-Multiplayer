namespace Subnautica.Events.EventArgs
{
    using System;

    public class BedIsCanSleepCheckingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BedIsCanSleepCheckingEventArgs(string uniqueId, Bed.BedSide side, bool isSeaTruckModule, bool isAllowed = true)
        {
            this.UniqueId         = uniqueId;
            this.Side             = side;
            this.IsAllowed        = isAllowed;
            this.IsSeaTruckModule = isSeaTruckModule;
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
        public Bed.BedSide Side { get; set; }

        /**
         *
         * IsSeaTruckModule Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSeaTruckModule { get; set; }

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

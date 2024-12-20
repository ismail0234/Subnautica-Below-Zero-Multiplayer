namespace Subnautica.Events.EventArgs
{
    using System;

    public class BedExitInUseModeEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BedExitInUseModeEventArgs(string uniqueId, TechType techType, bool isSeaTruckModule)
        {
            this.UniqueId         = uniqueId;
            this.TechType         = techType;
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
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * IsSeaTruckModule Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSeaTruckModule { get; set; }
    }
}

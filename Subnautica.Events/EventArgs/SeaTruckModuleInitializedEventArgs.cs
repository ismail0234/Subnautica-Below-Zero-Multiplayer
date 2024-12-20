namespace Subnautica.Events.EventArgs
{
    using System;

    public class SeaTruckModuleInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckModuleInitializedEventArgs(global::SeaTruckSegment module, TechType techType)
        {
            this.Module   = module;
            this.TechType = techType;
        }

        /**
         *
         * Module Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::SeaTruckSegment Module { get; set; }

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

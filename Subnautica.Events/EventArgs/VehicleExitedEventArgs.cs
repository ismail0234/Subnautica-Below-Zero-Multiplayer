namespace Subnautica.Events.EventArgs
{
    using System;

    public class VehicleExitedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleExitedEventArgs(string uniqueId, TechType techType)
        {
            this.UniqueId = uniqueId;
            this.TechType = techType;
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
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }
    }
}

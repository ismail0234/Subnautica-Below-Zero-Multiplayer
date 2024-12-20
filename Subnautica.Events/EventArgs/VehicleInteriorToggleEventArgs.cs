namespace Subnautica.Events.EventArgs
{
    using System;

    public class VehicleInteriorToggleEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleInteriorToggleEventArgs(string uniqueId, bool isEnter)
        {
            this.UniqueId  = uniqueId;
            this.IsEnter   = isEnter;
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
         * IsEnter Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEnter { get; private set; }
    }
}

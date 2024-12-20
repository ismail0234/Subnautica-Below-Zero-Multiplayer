namespace Subnautica.Events.EventArgs
{
    using System;

    public class PowerSourceRemovingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PowerSourceRemovingEventArgs(string uniqueId, IPowerInterface powerSource)
        {
            this.UniqueId    = uniqueId;
            this.PowerSource = powerSource;
        }

        /**
         *
         * ConstructionId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * SlotId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IPowerInterface PowerSource { get; set; }
    }
}

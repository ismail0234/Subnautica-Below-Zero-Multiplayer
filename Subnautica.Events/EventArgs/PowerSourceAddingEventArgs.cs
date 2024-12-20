namespace Subnautica.Events.EventArgs
{
    using System;

    public class PowerSourceAddingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PowerSourceAddingEventArgs(string uniqueId, IPowerInterface powerSource)
        {
            this.UniqueId = uniqueId;
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

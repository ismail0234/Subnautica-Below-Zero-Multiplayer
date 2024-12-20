namespace Subnautica.Events.EventArgs
{
    using System;

    public class BridgeTerminalClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BridgeTerminalClickingEventArgs(string uniqueId, bool isExtend, double time, bool isAllowed = true)
        {
            this.UniqueId = uniqueId;
            this.IsExtend = isExtend;
            this.Time     = (float) time;
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
         * IsExtend Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExtend { get; set; }

        /**
         *
         * Time Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Time { get; set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
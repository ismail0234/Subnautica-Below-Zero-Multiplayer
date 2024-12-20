namespace Subnautica.Events.EventArgs
{
    using System;

    public class LifepodZoneSelectingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LifepodZoneSelectingEventArgs(string key, bool isAllowed = true)
        {
            this.Key       = key;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * Key değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Key { get; set; }

        /**
         *
         * ZoneId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public sbyte ZoneId { get; set; } = -1;

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
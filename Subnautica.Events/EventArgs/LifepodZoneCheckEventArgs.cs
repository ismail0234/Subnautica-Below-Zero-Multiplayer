namespace Subnautica.Events.EventArgs
{
    using System;

    public class LifepodZoneCheckEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LifepodZoneCheckEventArgs(string key, bool isAllowed = true)
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
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
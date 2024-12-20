namespace Subnautica.Events.EventArgs
{
    using System;

    using static NotificationManager;

    public class NotificationToggleEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NotificationToggleEventArgs(Group group, string key, bool isAdded)
        {
            this.Group   = group;
            this.Key     = key;
            this.IsAdded = isAdded;
        }

        /**
         *
         * Group değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Group Group { get; set; }

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
         * IsAdded değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAdded { get; set; }
    }
}

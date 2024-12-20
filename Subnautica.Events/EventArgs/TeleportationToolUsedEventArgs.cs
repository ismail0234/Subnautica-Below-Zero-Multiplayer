namespace Subnautica.Events.EventArgs
{
    using System;

    public class TeleportationToolUsedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TeleportationToolUsedEventArgs(string teleporterId)
        {
            this.TeleporterId = teleporterId;
        }

        /**
         *
         * TeleporterId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TeleporterId { get; set; }
    }
}
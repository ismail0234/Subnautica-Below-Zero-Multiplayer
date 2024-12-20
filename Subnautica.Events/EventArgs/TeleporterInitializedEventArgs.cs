namespace Subnautica.Events.EventArgs
{
    using System;

    public class TeleporterInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TeleporterInitializedEventArgs(string uniqueId, string teleporterId, bool isExit)
        {
            this.UniqueId     = uniqueId;
            this.TeleporterId = teleporterId;
            this.IsExit       = isExit;
        }

        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * TeleporterId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TeleporterId { get; set; }

        /**
         *
         * IsExit Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExit { get; set; }
    }
}

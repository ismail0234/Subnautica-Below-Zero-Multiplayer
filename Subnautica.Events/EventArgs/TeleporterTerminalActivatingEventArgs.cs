namespace Subnautica.Events.EventArgs
{
    using System;

    public class TeleporterTerminalActivatingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TeleporterTerminalActivatingEventArgs(string uniqueId, string teleporterId, bool isAllowed = true)
        {
            this.UniqueId     = uniqueId;
            this.TeleporterId = teleporterId;
            this.IsAllowed    = isAllowed;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * TeleporterId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TeleporterId { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

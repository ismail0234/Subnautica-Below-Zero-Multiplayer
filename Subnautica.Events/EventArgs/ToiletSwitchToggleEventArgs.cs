namespace Subnautica.Events.EventArgs
{
    using System;

    public class ToiletSwitchToggleEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ToiletSwitchToggleEventArgs(string uniqueId, bool switchStatus, bool isAllowed = true)
        {
            this.UniqueId     = uniqueId;
            this.SwitchStatus = switchStatus;
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
         * SwitchStatus Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SwitchStatus { get; set; }

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

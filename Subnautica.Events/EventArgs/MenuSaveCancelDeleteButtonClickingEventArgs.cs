namespace Subnautica.Events.EventArgs
{
    using System;

    public class MenuSaveCancelDeleteButtonClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MenuSaveCancelDeleteButtonClickingEventArgs(string sessiondId, bool isRunAnimation = false, bool isAllowed = true)
        {
            SessionId = sessiondId;
            IsRunAnimation = isRunAnimation;
            IsAllowed = isAllowed;
        }

        /**
         *
         * SessionId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SessionId { get; set; }

        /**
         *
         * Animasyonun çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRunAnimation { get; set; }

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

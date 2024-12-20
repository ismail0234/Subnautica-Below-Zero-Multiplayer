namespace Subnautica.Events.EventArgs
{
    using System;

    public class MenuSaveLoadButtonClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MenuSaveLoadButtonClickingEventArgs(string sessiondId, bool isAllowed = true)
        {
            SessionId = sessiondId;
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
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

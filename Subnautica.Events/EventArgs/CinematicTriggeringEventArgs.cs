namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Enums;

    public class CinematicTriggeringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CinematicTriggeringEventArgs(string uniqueId, StoryCinematicType cinematicType, bool isClicked = false, bool isAllowed = true)
        {
            this.UniqueId           = uniqueId;
            this.StoryCinematicType = cinematicType;
            this.IsClicked          = isClicked;
            this.IsAllowed          = isAllowed;
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
         * StoryCinematicType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryCinematicType StoryCinematicType { get; private set; }

        /**
         *
         * IsClicked Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsClicked { get; private set; }

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

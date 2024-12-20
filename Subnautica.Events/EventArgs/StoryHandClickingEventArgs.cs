namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Enums;

    public class StoryHandClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryHandClickingEventArgs(string uniqueId, string goalKey, StoryCinematicType cinematicType, bool isAllowed = true)
        {
            this.UniqueId      = uniqueId;
            this.GoalKey       = goalKey;
            this.CinematicType = cinematicType;
            this.IsAllowed     = isAllowed;
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
         * GoalKey değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GoalKey { get; set; }
        /**
         *
         * CinematicType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryCinematicType CinematicType { get; set; }

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

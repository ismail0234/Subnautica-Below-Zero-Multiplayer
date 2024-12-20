namespace Subnautica.Events.EventArgs
{
    using System;

    public class UseableDiveHatchClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public UseableDiveHatchClickingEventArgs(string uniqueId, bool isEnter, string playerViewAnimation, bool isMoonpoolExpansion, bool isAllowed = true)
        {
            this.UniqueId            = uniqueId;
            this.IsEnter             = isEnter;
            this.IsBulkHead          = playerViewAnimation.Contains("surfacebasedoor_");
            this.IsLifePod           = playerViewAnimation.Contains("droppod_");
            this.IsMoonpoolExpansion = isMoonpoolExpansion;
            this.IsAllowed           = isAllowed;
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
         * IsEnter değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEnter { get; set; }

        /**
         *
         * IsBulkHead değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBulkHead { get; set; }

        /**
         *
         * IsLifePod değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLifePod { get; set; }

        /**
         *
         * IsMoonpoolExpansion değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMoonpoolExpansion { get; set; }

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
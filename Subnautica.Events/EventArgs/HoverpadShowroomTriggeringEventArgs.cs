namespace Subnautica.Events.EventArgs
{
    using System;

    public class HoverpadShowroomTriggeringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public HoverpadShowroomTriggeringEventArgs(string uniqueId, bool isEnter, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.IsEnter   = isEnter;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * IsEnter değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEnter { get; private set; }

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
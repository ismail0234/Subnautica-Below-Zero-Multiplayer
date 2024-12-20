namespace Subnautica.Events.EventArgs
{
    using System;

    public class BaseMapRoomResourceDiscoveringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseMapRoomResourceDiscoveringEventArgs(TechType techType, bool isAllowed = true)
        {
            this.TechType  = techType;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

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

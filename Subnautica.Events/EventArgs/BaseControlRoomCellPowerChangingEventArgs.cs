namespace Subnautica.Events.EventArgs
{
    using System;

    public class BaseControlRoomCellPowerChangingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseControlRoomCellPowerChangingEventArgs(string uniqueId, Int3 cell, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.Cell      = cell;
            this.IsAllowed = isAllowed;
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
         * Cell değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Int3 Cell { get; set; }

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

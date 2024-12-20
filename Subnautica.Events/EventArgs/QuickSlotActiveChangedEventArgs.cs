namespace Subnautica.Events.EventArgs
{
    using System;

    public class QuickSlotActiveChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public QuickSlotActiveChangedEventArgs(int slotId)
        {
            this.SlotId = slotId;
        }

        /**
         *
         * SlotId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int SlotId { get; private set; }
    }
}

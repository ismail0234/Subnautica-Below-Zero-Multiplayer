namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    public class EntitySlotSpawningEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntitySlotSpawningEventArgs(int slotId, bool isAllowed = true)
        {
            this.SlotId    = slotId;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int SlotId { get; set; }

        /**
         *
         * ClassId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ClassId { get; set; }

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

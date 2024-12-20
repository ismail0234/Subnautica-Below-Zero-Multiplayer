namespace Subnautica.Events.EventArgs
{
    using System;

    public class UpgradeConsoleModuleRemovedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public UpgradeConsoleModuleRemovedEventArgs(string uniqueId, string slotId, string itemId, TechType moduleType)
        {
            this.UniqueId    = uniqueId;
            this.SlotId      = slotId;
            this.ItemId      = itemId;
            this.ModuleType  = moduleType;
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
         * SlotId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SlotId { get; set; }

        /**
         *
         * ItemId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ItemId { get; set; }

        /**
         *
         * ModuleType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType ModuleType { get; set; }
    }
}

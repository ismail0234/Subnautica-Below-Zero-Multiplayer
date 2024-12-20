namespace Subnautica.Events.EventArgs
{
    using System;

    public class ToolBatteryEnergyChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ToolBatteryEnergyChangedEventArgs(string uniqueId, global::Pickupable item)
        {
            this.UniqueId = uniqueId;
            this.Item     = item;
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
         * Item Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Pickupable Item { get; private set; }
    }
}
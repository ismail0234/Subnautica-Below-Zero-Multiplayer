namespace Subnautica.Events.EventArgs
{
    using System;

    public class GrownPlantHarvestingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GrownPlantHarvestingEventArgs(string uniqueId, GrownPlant grownPlant, bool isAllowed = true)
        {
            this.UniqueId   = uniqueId;
            this.GrownPlant = grownPlant;
            this.IsAllowed  = isAllowed;
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
         * GrownPlant Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GrownPlant GrownPlant { get; private set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

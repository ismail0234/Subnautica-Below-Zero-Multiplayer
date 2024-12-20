namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class PlanterProgressCompletedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlanterProgressCompletedEventArgs(Plantable plantable, GameObject grownPlant)
        {
            this.Plantable  = plantable;
            this.GrownPlant = grownPlant;
        }

        /**
         *
         * Plantable Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Plantable Plantable { get; private set; }

        /**
         *
         * GrownPlant Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GrownPlant { get; private set; }
    }
}


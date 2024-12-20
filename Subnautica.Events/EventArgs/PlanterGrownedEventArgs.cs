namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class PlanterGrownedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlanterGrownedEventArgs(FruitPlant fruitPlant)
        {
            this.FruitPlant = fruitPlant;
        }

        /**
         *
         * FruitPlant Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FruitPlant FruitPlant { get; private set; }
    }
}

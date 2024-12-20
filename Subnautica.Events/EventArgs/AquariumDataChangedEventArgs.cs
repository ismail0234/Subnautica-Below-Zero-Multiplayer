namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections.Generic;

    public class AquariumDataChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AquariumDataChangedEventArgs(string uniqueId, List<TechType> fishes)
        {
            this.UniqueId = uniqueId;
            this.Fishes   = fishes;
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
         * Fishes Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<TechType> Fishes { get; private set; }
    }
}
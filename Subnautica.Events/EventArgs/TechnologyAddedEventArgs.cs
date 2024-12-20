namespace Subnautica.Events.EventArgs
{
    using System;

    public class TechnologyAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechnologyAddedEventArgs(TechType type, bool verbose)
        {
            this.TechType = type;
            this.Verbose  = verbose;
        }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Verbose Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Verbose { get; private set; }
    }
}

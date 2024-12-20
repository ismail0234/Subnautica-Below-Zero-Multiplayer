namespace Subnautica.Events.EventArgs
{
    using System;

    public class TechAnalyzeAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechAnalyzeAddedEventArgs(TechType techType, bool verbose)
        {
            this.TechType = techType;
            this.Verbose  = verbose;
        }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Verbose değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Verbose { get; private set; }
    }
}

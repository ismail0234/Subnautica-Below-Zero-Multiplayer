namespace Subnautica.Events.EventArgs
{
    using System;

    public class EncyclopediaAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EncyclopediaAddedEventArgs(string key, bool verbose)
        {
            this.Key = key;
            this.Verbose = verbose;
        }

        /**
         *
         * Key Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Key { get; private set; }

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

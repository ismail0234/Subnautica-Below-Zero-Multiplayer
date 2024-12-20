namespace Subnautica.Events.EventArgs
{
    using System;

    public class PDALogAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PDALogAddedEventArgs(string key, float timestamp)
        {
            this.Key = key;
            this.Timestamp = timestamp;
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
         * Timestamp Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Timestamp { get; private set; }
    }
}

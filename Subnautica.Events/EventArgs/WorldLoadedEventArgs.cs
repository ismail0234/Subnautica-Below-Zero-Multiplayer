namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class WorldLoadedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldLoadedEventArgs()
        {
            this.WaitingMethods = new List<IEnumerator>();
        }

        /**
         *
         * WaitingMethods Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<IEnumerator> WaitingMethods { get; set; }
    }
}

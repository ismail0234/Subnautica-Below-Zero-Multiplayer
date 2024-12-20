namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class WorldLoadingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldLoadingEventArgs(IEnumerator method = null)
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
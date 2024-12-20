namespace Subnautica.Events.EventArgs
{
    using System;

    public class OxygenPlantClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public OxygenPlantClickingEventArgs(string uniqueId, float startedTime)
        {
            this.UniqueId    = uniqueId;
            this.StartedTime = startedTime;
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
         * StartedTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float StartedTime { get; private set; }
    }
}

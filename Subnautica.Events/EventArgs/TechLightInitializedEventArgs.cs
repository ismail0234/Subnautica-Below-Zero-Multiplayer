namespace Subnautica.Events.EventArgs
{
    using System;

    public class TechLightInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechLightInitializedEventArgs(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }
    }
}
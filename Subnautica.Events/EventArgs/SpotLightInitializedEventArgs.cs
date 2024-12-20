namespace Subnautica.Events.EventArgs
{
    using System;

    public class SpotLightInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpotLightInitializedEventArgs(string uniqueId)
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
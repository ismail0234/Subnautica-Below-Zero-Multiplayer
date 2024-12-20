namespace Subnautica.Events.EventArgs
{
    using System;

    public class SealedInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SealedInitializedEventArgs(string uniqueId, Sealed sealedObject)
        {
            this.UniqueId     = uniqueId;
            this.SealedObject = sealedObject;
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
         * SealedObject Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Sealed SealedObject { get; private set; }
    }
}

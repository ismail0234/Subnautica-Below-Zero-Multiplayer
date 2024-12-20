namespace Subnautica.Events.EventArgs
{
    using System;

    public class EntityScannerCompletedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntityScannerCompletedEventArgs(string uniqueId, TechType techType)
        {
            this.UniqueId = uniqueId;
            this.TechType = techType;
        }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }
    }
}

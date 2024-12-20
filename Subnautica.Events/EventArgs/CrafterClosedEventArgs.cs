namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrafterClosedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrafterClosedEventArgs(string uniqueId, TechType fabricatorType)
        {
            this.UniqueId = uniqueId;
            this.FabricatorType = fabricatorType;
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
         * FabricatorType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType FabricatorType { get; private set; }
    }
}
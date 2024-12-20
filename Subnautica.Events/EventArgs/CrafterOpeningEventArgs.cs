namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrafterOpeningEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrafterOpeningEventArgs(string uniqueId, TechType fabricatorType, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.FabricatorType = fabricatorType;
            this.IsAllowed      = isAllowed;
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

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
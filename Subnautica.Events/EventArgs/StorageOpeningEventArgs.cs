namespace Subnautica.Events.EventArgs
{
    using System;

    public class StorageOpeningEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StorageOpeningEventArgs(string constructionId, TechType techType, bool isAllowed = true)
        {
            this.ConstructionId = constructionId;
            this.TechType       = techType;
            this.IsAllowed      = isAllowed;
        }

        /**
         *
         * ConstructionId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ConstructionId { get; set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

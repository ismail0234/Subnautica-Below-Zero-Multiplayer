namespace Subnautica.Events.EventArgs
{
    using System;

    public class SignSelectEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SignSelectEventArgs(string uniqueId, TechType techType, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.TechType  = techType;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

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

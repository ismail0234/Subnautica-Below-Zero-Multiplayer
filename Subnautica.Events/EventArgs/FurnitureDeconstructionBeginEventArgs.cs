namespace Subnautica.Events.EventArgs
{
    using System;

    public class FurnitureDeconstructionBeginEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FurnitureDeconstructionBeginEventArgs(string uniqueId, TechType techType, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.TechType  = techType;
            this.IsAllowed = isAllowed;
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

namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrafterBeginEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrafterBeginEventArgs(string uniqueId, TechType fabricatorType, TechType techType, float duration, bool isAllowed = true)
        {
            this.UniqueId = uniqueId;
            this.FabricatorType = fabricatorType;
            this.TechType = techType;
            this.Duration = duration;
            this.IsAllowed = isAllowed;
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
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Duration Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Duration { get; private set; }

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

namespace Subnautica.Events.EventArgs
{
    using System;

    public class CrafterItemPickupEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrafterItemPickupEventArgs(string uniqueId, global::GhostCrafter crafter, int amount, TechType fabricatorType, TechType techType, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.Crafter        = crafter;
            this.Amount         = amount;
            this.FabricatorType = fabricatorType;
            this.TechType       = techType;
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
         * Crafter Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::GhostCrafter Crafter { get; private set; }

        /**
         *
         * amount Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int Amount { get; private set; }

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
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}

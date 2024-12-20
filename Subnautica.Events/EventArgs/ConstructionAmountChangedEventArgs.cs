namespace Subnautica.Events.EventArgs
{
    using System;
    public class ConstructionAmountChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ConstructionAmountChangedEventArgs(TechType techType, float constructedAmount, bool isConstruct, string uniqueId)
        {
            this.TechType    = techType;
            this.UniqueId    = uniqueId;
            this.IsConstruct = isConstruct;
            this.Amount      = constructedAmount;
        }

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
         * Amount Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Amount { get; private set; }

        /**
         *
         * IsConstruct Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsConstruct { get; private set; }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }
    }
}

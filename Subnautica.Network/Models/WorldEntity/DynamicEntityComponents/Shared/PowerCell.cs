namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared
{
    using MessagePack;

    using UnityEngine;

    [MessagePackObject]
    public class PowerCell
    {
        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * Charge Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float Charge { get; set; } = 200f;

        /**
         *
         * Capacity Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float Capacity { get; set; } = 200f;

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public TechType TechType { get; set; } = TechType.PowerCell;

        /**
         *
         * Full mü?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsFull
        {
            get
            {
                return this.Charge == this.Capacity;
            }
        }

        /**
         *
         * Mevcut mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsExists
        {
            get
            {
                return this.Charge != -1f;
            }
        }

        /**
         *
         * Enerji ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddEnergy(float energyAmount, out float usedEnergyAmount)
        {
            if (this.IsFull || !this.IsExists)
            {
                usedEnergyAmount = 0f;
                return false;
            }

            usedEnergyAmount = Mathf.Min(this.Capacity - this.Charge, energyAmount);

            this.Charge += usedEnergyAmount;
            return true;
        }

        /**
         *
         * Enerji tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConsumeEnergy(float energyAmount, out float usedEnergyAmount)
        {
            usedEnergyAmount = 0f;

            if (this.Charge > 0f && energyAmount > 0f)
            {
                var oldCharge = this.Charge;

                this.Charge = Mathf.Max(0f, oldCharge - energyAmount);

                usedEnergyAmount = oldCharge - this.Charge; 
                return true;
            }

            return false;
        }

        /**
         *
         * Batarya türünü değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBatteryType(TechType techType)
        {
            this.TechType = techType;
            this.Capacity = techType == TechType.PrecursorIonPowerCell ? 1000f : 200f;
        }
    }
}

namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using System;

    using MessagePack;

    [MessagePackObject]
    public class BatteryItem
    {
        /**
         *
         * Mevcut slotu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsActive { get; set; } = false;

        /**
         *
         * Mevcut slotu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string SlotId { get; set; }

        /**
         *
         * Mevcut teknolojiyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public TechType TechType { get; set; }

        /**
         *
         * Mevcut şarj durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float Charge { get; set; }

        /**
         *
         * Max şarj durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float Capacity { get; set; } = 100f;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BatteryItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BatteryItem(string slotId = null, TechType techType = TechType.None, float charge = 0f, float capacity = 100f)
        {
            this.SlotId   = slotId;
            this.TechType = techType;
            this.Charge   = charge;
            this.Capacity = capacity;
        }

        /**
         *
         * Batarya bilgilerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetSlotId()
        {
            switch (this.TechType)
            {
                case TechType.Battery:
                case TechType.PrecursorIonBattery:
                    return Byte.Parse(this.SlotId.Replace("BatteryCharger", ""));

                case TechType.PowerCell:
                case TechType.PrecursorIonPowerCell:
                    return Byte.Parse(this.SlotId.Replace("PowerCellCharger", ""));
            }

            return 0;
        }

        /**
         *
         * Batarya bilgilerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBattery(TechType techType, float charge)
        {
            this.Clear();

            if (techType != TechType.None)
            {
                this.IsActive = true;
                this.TechType = techType;
                this.Charge   = charge;
                this.Capacity = this.GetCapacity();
            }
        }

        /**
         *
         * Sınıfı temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetCapacity()
        {
            switch (this.TechType)
            {
                case TechType.Battery:
                    return 100f;
                case TechType.PowerCell:
                    return 200f;
                case TechType.PrecursorIonBattery:
                    return 500f;
                case TechType.PrecursorIonPowerCell:
                    return 1000f;
            }

            return 100f;
        }

        /**
         *
         * Sınıfı temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Clear()
        {
            this.IsActive = false;
            this.TechType = TechType.None;
            this.Charge   = 0f;
            this.Capacity = 0f;
        }
    }
}
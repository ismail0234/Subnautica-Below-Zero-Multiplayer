namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class CoffeeVendingMachine : MetadataComponent
    {
        /**
         *
         * IsAdding değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsAdding { get; set; }

        /**
         *
         * IsFull değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsFull { get; set; }

        /**
         *
         * WasPowered değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool WasPowered { get; set; }

        /**
         *
         * ItemId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public string ItemId { get; set; }

        /**
         *
         * Thermoses değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public List<CoffeeThermos> Thermoses { get; set; } = new List<CoffeeThermos> ()
        { 
            new CoffeeThermos(),
            new CoffeeThermos(),
        };

        /**
         *
         * PickupItem değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public WorldPickupItem PickupItem { get; set; }
    }

    [MessagePackObject]
    public class CoffeeThermos
    {
        /**
         *
         * IsActive değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsActive { get; set; } = false;

        /**
         *
         * ItemId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string ItemId { get; set; } = null;

        /**
         *
         * IsFull değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsFull { get; set; } = false;

        /**
         *
         * AddedTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float AddedTime { get; set; } = 0.0f;

        /**
         *
         * Thermos'u ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Initialize(string itemId, float currentTime)
        {
            this.IsActive  = true;
            this.IsFull    = false;
            this.ItemId    = itemId;
            this.AddedTime = currentTime;
        }

        /**
         *
         * Thermos'u doldurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Refill()
        {
            this.IsFull = true;
        }

        /**
         *
         * Thermos'u sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Clear()
        {
            this.IsActive  = false;
            this.IsFull    = false;
            this.ItemId    = null;
            this.AddedTime = 0.0f;
        }
    }
}

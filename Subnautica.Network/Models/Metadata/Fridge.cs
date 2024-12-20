namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class Fridge : MetadataComponent
    {
        /**
         *
         * IsAdded değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsAdded { get; set; }

        /**
         *
         * IsPowerStateChanged değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsPowerStateChanged { get; set; }

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
         * CurrentTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float CurrentTime { get; set; }

        /**
         *
         * IsDecomposes değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsDecomposes { get; set; }

        /**
         *
         * TimeDecayStart değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float TimeDecayStart { get; set; } = 0.0f;

        /**
         *
         * ItemComponent değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public FridgeItemComponent ItemComponent { get; set; }

        /**
         *
         * WorldPickupItem değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public WorldPickupItem WorldPickupItem { get; set; }

        /**
         *
         * StorageContainer değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public Metadata.StorageContainer StorageContainer { get; set; }

        /**
         *
         * Components değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public List<FridgeItemComponent> Components { get; set; } = new List<FridgeItemComponent>();
    }

    [MessagePackObject]
    public class FridgeItemComponent
    {
        /**
         *
         * ItemId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string ItemId { get; set; }

        /**
         *
         * IsPaused değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsPaused { get; set; }

        /**
         *
         * IsDecomposes değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsDecomposes { get; set; }

        /**
         *
         * TimeDecayPause değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float TimeDecayPause { get; set; } = 0.0f;

        /**
         *
         * TimeDecayStart değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float TimeDecayStart { get; set; } = 0.0f;

        /**
         *
         * Çürümeyi durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void PauseDecay(float serverTime)
        {
            if (!this.IsPaused)
            {
                this.IsPaused = true;
                this.TimeDecayPause = serverTime;
            }
        }

        /**
         *
         * Çürümeyi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UnpauseDecay(float serverTime)
        {
            if (this.IsPaused)
            {
                this.IsPaused = false;
                this.TimeDecayStart += serverTime - this.TimeDecayPause;
           }
        }
    }
}
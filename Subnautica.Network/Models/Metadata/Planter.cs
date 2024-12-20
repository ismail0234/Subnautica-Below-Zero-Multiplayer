namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Planter : MetadataComponent
    {
        /**
         *
         * IsOpening Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsOpening { get; set; }

        /**
         *
         * IsAdding Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsAdding { get; set; }

        /**
         *
         * IsHarvesting Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsHarvesting { get; set; }

        /**
         *
         * CurrentItem Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public PlanterItem CurrentItem { get; set; }

        /**
         *
         * Items Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public List<PlanterItem> Items { get; set; } = new List<PlanterItem>();
    }

    [MessagePackObject]
    public class PlanterItem
    {
        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public TechType TechType { get; set; }

        /**
         *
         * ItemId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string ItemId { get; set; }

        /**
         *
         * SlotId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public byte SlotId { get; set; }

        /**
         *
         * TimeStartGrowth Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float TimeStartGrowth { get; set; } = -1f;

        /**
         *
         * Health Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float Health { get; set; } = -1f;

        /**
         *
         * Duration Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public short Duration { get; set; } = -1;

        /**
         *
         * ActiveFruitCount Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public byte ActiveFruitCount { get; set; } = 0;

        /**
         *
         * TimeNextFruit Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float TimeNextFruit { get; set; } = 0f;

        /**
         *
         * MaxSpawnableFruit Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public byte MaxSpawnableFruit { get; set; } = 0;

        /**
         *
         * FruitSpawnInterval Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public float FruitSpawnInterval { get; set; } = 0f;

        /**
         *
         * Meyve bilgilerini senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SyncFruits(float currentTime)
        {
            while (this.ActiveFruitCount < this.MaxSpawnableFruit && currentTime >= this.TimeNextFruit)
            {
                this.TimeNextFruit += this.FruitSpawnInterval;

                this.ActiveFruitCount++;
            }

            if (this.ActiveFruitCount >= this.MaxSpawnableFruit)
            {
                this.TimeNextFruit = currentTime + this.FruitSpawnInterval;
            }
        }
    }
}

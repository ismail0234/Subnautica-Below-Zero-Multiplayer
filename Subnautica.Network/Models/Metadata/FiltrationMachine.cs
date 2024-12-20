namespace Subnautica.Network.Models.Metadata
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class FiltrationMachine : MetadataComponent
    {
        /**
         *
         * IsUnderwater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsUnderwater { get; set; } = true;

        /**
         *
         * RemovingItemId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string RemovingItemId { get; set; }

        /**
         *
         * TimeRemainingWater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float TimeRemainingWater { get; set; } = 840f;

        /**
         *
         * TimeRemainingSalt değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float TimeRemainingSalt { get; set; } = 420f;

        /**
         *
         * TimeRemainingSalt değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public FiltrationMachineItem Item { get; set; }

        /**
         *
         * Thermoses değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public List<FiltrationMachineItem> Items { get; set; } = new List<FiltrationMachineItem>()
        {
            new FiltrationMachineItem(TechType.BigFilteredWater),
            new FiltrationMachineItem(TechType.BigFilteredWater),
            new FiltrationMachineItem(TechType.Salt),
            new FiltrationMachineItem(TechType.Salt),
        };
    }

    [MessagePackObject]
    public class FiltrationMachineItem
    {
        /**
         *
         * IsActive değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public TechType TechType { get; set; }

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
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FiltrationMachineItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FiltrationMachineItem(TechType techType)
        {
            this.TechType = techType;
        }

        /**
         *
         * Temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Clear()
        {
            this.ItemId = null;
        }
    }

    [MessagePackObject]
    public class FiltrationMachineTimeItem
    {
        /**
         *
         * TimeRemainingWater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public uint ConstructionIndex { get; set; }

        /**
         *
         * TimeRemainingWater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float TimeRemainingWater { get; set; }

        /**
         *
         * TimeRemainingSalt değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float TimeRemainingSalt { get; set; }

        /**
         *
         * Items Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FiltrationMachineTimeItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FiltrationMachineTimeItem(uint constructionIndex, float timeRemainingWater, float timeRemainingSalt, ZeroVector3 position)
        {
            this.ConstructionIndex  = constructionIndex;
            this.TimeRemainingWater = timeRemainingWater;
            this.TimeRemainingSalt  = timeRemainingSalt;
            this.Position = position;
        }
    }
}

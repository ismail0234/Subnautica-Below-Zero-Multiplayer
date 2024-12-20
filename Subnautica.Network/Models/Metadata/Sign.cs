namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Sign : MetadataComponent
    {
        /**
         *
         * Metni barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string Text { get; set; }

        /**
         *
         * ElementsState değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool[] ElementsState { get; set; }

        /**
         *
         * ScaleIndex değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public int ScaleIndex { get; set; }

        /**
         *
         * ColorIndex değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public int ColorIndex { get; set; }

        /**
         *
         * IsBackgroundEnabled değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsBackgroundEnabled { get; set; }

        /**
         *
         * IsOpening değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsOpening { get; set; }

        /**
         *
         * IsSave değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsSave { get; set; }
    }
}

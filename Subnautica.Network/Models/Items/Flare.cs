namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Flare : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.Flare;

        /**
         *
         * Position değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Forward değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroVector3 Forward { get; set; }
        /**
         *
         * Rotation değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroQuaternion Rotation { get; set; }
        /**
         *
         * Intensity değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float Intensity { get; set; }

        /**
         *
         * Range değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public float Range { get; set; }

        /**
         *
         * Energy değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float Energy { get; set; }

        /**
         *
         * Entity Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public WorldDynamicEntity Entity { get; set; }
    }
}
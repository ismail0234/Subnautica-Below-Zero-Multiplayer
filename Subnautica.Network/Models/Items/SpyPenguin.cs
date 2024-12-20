namespace Subnautica.Network.Models.Items
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class SpyPenguin : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.SpyPenguin;

        /**
         *
         * Position Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Rotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Name Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public string Name { get; set; }

        /**
         *
         * Items Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float Health { get; set; }

        /**
         *
         * Items Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public List<byte[]> Items { get; set; }

        /**
         *
         * StalkerChance Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float SpawnChance { get; set; } = -1f;

        /**
         *
         * WorldPickupItem Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public WorldPickupItem WorldPickupItem { get; set; }

        /**
         *
         * Entity Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public WorldDynamicEntity Entity { get; set; }

        /**
         *
         * IsPickup Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public bool IsPickup { get; set; }

        /**
         *
         * IsStalkerFur Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public bool IsStalkerFur { get; set; }

        /**
         *
         * IsDeploy Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public bool IsDeploy { get; set; }

        /**
         *
         * IsAdded Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public bool IsAdded { get; set; }
    }
}
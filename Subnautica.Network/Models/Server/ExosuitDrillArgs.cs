namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using EntityModel = Subnautica.Network.Models.WorldEntity;
    
    [MessagePackObject]
    public class ExosuitDrillArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.ExosuitDrill;

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string UniqueId { get; set; }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string SlotId { get; set; }

        /**
         *
         * MaxHealth Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float MaxHealth { get; set; }

        /**
         *
         * NewHealth Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float NewHealth { get; set; }

        /**
         *
         * DropTechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public TechType DropTechType { get; set; }

        /**
         *
         * DropPositions Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public List<ZeroVector3> DropPositions { get; set; }

        /**
         *
         * InventoryItems Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public List<WorldPickupItem> InventoryItems { get; set; } = new List<WorldPickupItem>();

        /**
         *
         * WorldItems Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public List<WorldDynamicEntity> WorldItems { get; set; } = new List<WorldDynamicEntity>();

        /**
         *
         * DisableItem Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public WorldPickupItem DisableItem { get; set; }

        /**
         *
         * IsMultipleDrill Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public bool IsMultipleDrill { get; set; }

        /**
         *
         * IsStaticWorldEntity Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public bool IsStaticWorldEntity { get; set; }

        /**
         *
         * StaticEntity Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(16)]
        public EntityModel.Drillable StaticEntity { get; set; }
    }
}
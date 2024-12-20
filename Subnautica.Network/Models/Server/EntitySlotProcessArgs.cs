namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class EntitySlotProcessArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.EntitySlotProcess;

        /**
         *
         * IsBreakable Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsBreakable { get; set; }

        /**
         *
         * ZeroVector3 Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Entity Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public WorldDynamicEntity Entity { get; set; }

        /**
         *
         * WorldPickupItem Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public WorldPickupItem WorldPickupItem { get; set; }
    }
}
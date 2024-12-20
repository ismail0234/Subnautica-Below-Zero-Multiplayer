namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class SeaTruckDockingModuleArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.SeaTruckDockingModule;

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
         * VehicleId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string VehicleId { get; set; }

        /**
         *
         * Vehicle Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public WorldDynamicEntity Vehicle { get; set; }

        /**
         *
         * IsDocking Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public bool IsDocking { get; set; }

        /**
         *
         * IsEnterUndock Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsEnterUndock { get; set; }

        /**
         *
         * IsEnterUndock Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public ZeroVector3 UndockPosition { get; set; }

        /**
         *
         * IsEnterUndock Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public ZeroQuaternion UndockRotation { get; set; }
    }
}
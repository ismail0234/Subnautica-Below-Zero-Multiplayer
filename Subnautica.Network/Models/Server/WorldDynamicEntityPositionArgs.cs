namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;

    [MessagePackObject]
    public class WorldDynamicEntityPositionArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.WorldDynamicEntityPosition;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.EntityMovement;

        /**
         *
         * Packet Teslim Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Unreliable;

        /**
         *
         * Key Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public List<WorldDynamicEntityPosition> Positions { get; set; }
    }
}
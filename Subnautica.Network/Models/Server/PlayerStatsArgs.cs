namespace Subnautica.Network.Models.Server
{
    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class PlayerStatsArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.PlayerStats;

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
         * Mevcut Sağlık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float Health { get; set; }

        /**
         *
         * Su Miktarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public float Water { get; set; }

        /**
         *
         * Açlık Miktarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float Food { get; set; }
    }
}

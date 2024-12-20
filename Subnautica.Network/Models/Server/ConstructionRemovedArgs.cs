namespace Subnautica.Network.Models.Server
{
    using MessagePack;
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class ConstructionRemovedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.ConstructingRemoved;
        
        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.Construction;

        /**
         *
         * Packet Kanal Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public override byte ChannelId { get; set; } = 1;

        /**
         *
         * Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public TechType TechType { get; set; }

        /**
         *
         * Cell Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroInt3 Cell { get; set; }

        /**
         *
         * Ağ Kimliği Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public string UniqueId { get; set; }
    }
}

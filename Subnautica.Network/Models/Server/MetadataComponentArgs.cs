namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class MetadataComponentArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.MetadataRequest;

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
         * UniqueId verisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string UniqueId { get; set; }

        /**
         *
         * IsStartupNull verisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsStartupNull { get; set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public TechType TechType { get; set; }

        /**
         *
         * SecretTechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public TechType SecretTechType { get; set; }

        /**
         *
         * Metadata verisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public MetadataComponent Component { get; set; }
    }
}
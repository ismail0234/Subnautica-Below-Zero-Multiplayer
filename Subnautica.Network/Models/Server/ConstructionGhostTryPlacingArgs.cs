namespace Subnautica.Network.Models.Server
{
    using MessagePack;
    
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Construction.Shared;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class ConstructionGhostTryPlacingArgs : NetworkPacket
    {        
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.ConstructingGhostTryPlacing;
        
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
         * Ağ Kimliği Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string UniqueId { get; set; }

        /**
         *
         * SubrootId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string SubrootId { get; set; }

        /**
         *
         * Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public TechType TechType { get; set; }

        /**
         *
         * Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public int LastRotation { get; set; }

        /**
         *
         * Nesne Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Nesne Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * AimTransform
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public ZeroTransform AimTransform { get; set; }

        /**
         *
         * BaseGhostComponent
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public BaseGhostComponent BaseGhostComponent { get; set; }

        /**
         *
         * IsCanPlace Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public bool IsCanPlace { get; set; }

        /**
         *
         * IsBasePiece Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public bool IsBasePiece { get; set; }

        /**
         *
         * IsError Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public bool IsError { get; set; }
    }
}

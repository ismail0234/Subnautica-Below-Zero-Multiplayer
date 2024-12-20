namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class ConstructionCompletedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.ConstructingCompleted;
        
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
         * Yapı Id Kimliği Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public uint Id { get; set; }

        /**
         *
         * Ağ Kimliği Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string UniqueId { get; set; }

        /**
         *
         * Base Kimliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public string BaseId { get; set; }

        /**
         *
         * Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public TechType TechType { get; set; }

        /**
         *
         * CellPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public ZeroVector3 CellPosition { get; set; }

        /**
         *
         * LocalPosition Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public ZeroVector3 LocalPosition { get; set; }

        /**
         *
         * LocalRotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public ZeroQuaternion LocalRotation { get; set; }

        /**
         *
         * IsFaceHasValue Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public bool IsFaceHasValue { get; set; }

        /**
         *
         * FaceDirection Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public Base.Direction FaceDirection { get; set; }

        /**
         *
         * FaceType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public Base.FaceType FaceType { get; set; }
    }
}

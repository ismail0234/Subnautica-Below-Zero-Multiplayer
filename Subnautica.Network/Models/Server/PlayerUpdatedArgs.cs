namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;
    
    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class PlayerUpdatedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.PlayerUpdated;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.PlayerMovement;

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
         * Sıkıştırılmış kamera açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public short CompressedCameraPitch { get; set; }

        /**
         *
         * Nesne Pozisyonu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public long CompressedPosition { get; set; }

        /**
         *
         * Nesne Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public long CompressedRotation { get; set; }


        /**
         *
         * Lokal Pozisyon
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public long CompressedLocalPosition { get; set; }

        /**
         *
         * CompressedRightHandItemRotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public long CompressedRightHandItemRotation { get; set; }

        /**
         *
         * CompressedLeftHandItemRotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public long CompressedLeftHandItemRotation { get; set; }

        /**
         *
         * HandItemComponent değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public int CompressedCameraForward { get; set; }

        /**
         *
         * Oyuncu Elindeki Eşya
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public TechType ItemInHand { get; set; }

        /**
         *
         * SurfaceType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public VFXSurfaceTypes SurfaceType { get; set; }

        /**
         *
         * IsPrecursorArm değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public bool IsPrecursorArm { get; set; }

        /**
         *
         * EmoteIndex değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public byte EmoteIndex { get; set; }
        /**
         *
         * Ekipmanları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(16)]
        public List<TechType> Equipments { get; set; }

        /**
         *
         * HandItemComponent değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(17)]
        public NetworkPlayerItemComponent HandItemComponent { get; set; }
    }
}
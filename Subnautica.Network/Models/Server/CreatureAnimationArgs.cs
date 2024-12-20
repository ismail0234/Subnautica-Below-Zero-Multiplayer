namespace Subnautica.Network.Models.Server
{
    using LiteNetLib;
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    using System.Collections.Generic;

    [MessagePackObject]
    public class CreatureAnimationArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.CreatureAnimation;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.FishMovement;

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
         * Animations Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public HashSet<CreatureAnimationItem> Animations { get; set; } = new HashSet<CreatureAnimationItem>();
    }

    [MessagePackObject]
    public class CreatureAnimationItem
    {
        /**
         *
         * CreatureId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ushort CreatureId { get; set; }

        /**
         *
         * Animations Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public Dictionary<byte, byte> Animations = new Dictionary<byte, byte>();
    }
}

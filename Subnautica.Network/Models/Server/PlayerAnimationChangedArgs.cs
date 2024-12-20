namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class PlayerAnimationChangedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.PlayerAnimationChanged;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.PlayerAnimation;

        /**
         *
         * Değişen Animasyonlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public Dictionary<PlayerAnimationType, bool> Animations { get; set; }
    }
}



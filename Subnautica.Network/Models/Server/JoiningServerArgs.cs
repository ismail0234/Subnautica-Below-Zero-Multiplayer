namespace Subnautica.Network.Models.Server
{
    using MessagePack;
    
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class JoiningServerArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.JoiningServer;

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override NetworkChannel ChannelType { get; set; } = NetworkChannel.Startup;

        /**
         *
         * Oyuncu ID
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string UserId { get; set; }

        /**
         *
         * Oyuncu Nicki
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public string UserName { get; set; }

        /**
         *
         * Yeniden Bağlantı Durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public bool IsReconnect { get; set; } = false;
    }
}
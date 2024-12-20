namespace Subnautica.Network.Models.Client
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Player;
    using Subnautica.Network.Models.WorldStreamer;

    [MessagePackObject]
    public class WorldLoadedArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.WorldLoaded;

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
         * IsSpawnPointRequest Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsSpawnPointRequest { get; set; }

        /**
         *
         * IsSpawnPointExists Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsSpawnPointExists { get; set; }

        /**
         *
         * Resim İsimleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)] 
        public Dictionary<string, Metadata.PictureFrame> Images { get; set; } = new Dictionary<string, Metadata.PictureFrame>(); 

        /**
         *
         * Resim İsimleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public List<string> ExistImages { get; set; } = new List<string>();

        /**
         *
         * Bağlı Oyuncular İsimleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public List<PlayerItem> Players { get; set; } = new List<PlayerItem>();

        /**
         *
         * SpawnPoints Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public HashSet<ZeroSpawnPointSimple> SpawnPoints { get; set; } = new HashSet<ZeroSpawnPointSimple>();
    }
}

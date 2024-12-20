namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    using WorldChildrens = Subnautica.Network.Models.Storage.World.Childrens;

    [MessagePackObject]
    public class IntroStartArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.FirstTimeStartServer;

        /**
         *
         * SupplyDrop Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public WorldChildrens.SupplyDrop SupplyDrop { get; set; }

        /**
         *
         * IsFinished Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsFinished { get; set; }

        /**
         *
         * ServerTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float ServerTime { get; set; }
    }
}

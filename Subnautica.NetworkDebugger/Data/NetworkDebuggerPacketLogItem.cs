namespace Subnautica.NetworkDebugger.Data
{
    using LiteNetLib;

    using Subnautica.API.Enums;

    public class NetworkDebuggerPacketLogItem
    {
        /**
         *
         * Boyut Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int Size { get; set; }

        /**
         *
         * Channel Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetworkChannel Channel { get; set; }

        /**
         *
         * DeliveryMethod Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public DeliveryMethod DeliveryMethod { get; set; }

        /**
         *
         * IsDownload Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDownload { get; set; }

        /**
         *
         * IsClient Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsClient { get; set; }
    }
}

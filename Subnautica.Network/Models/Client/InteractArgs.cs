namespace Subnautica.Network.Models.Client
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class InteractArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.Interact;

        /**
         *
         * Bloklu listeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public Dictionary<string, string> List { get; set; }
    }
}
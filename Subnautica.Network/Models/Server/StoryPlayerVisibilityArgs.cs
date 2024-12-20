namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class StoryPlayerVisibilityArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.StoryPlayerVisibility;

        /**
         *
         * IsCinematicActive Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsCinematicActive { get; set; }

        /**
         *
         * Visibility Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public Dictionary<string, bool> Visibility { get; set; } = new Dictionary<string, bool>();
    }
}

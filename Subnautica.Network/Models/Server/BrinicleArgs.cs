namespace Subnautica.Network.Models.Server
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;

    [MessagePackObject]
    public class BrinicleArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.Brinicle;

        /**
         *
         * UniqueIds Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public List<Brinicle> WaitingForRegistry { get; set; } = new List<Brinicle>();

        /**
         *
         * Brinicles Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public List<Brinicle> Brinicles { get; set; } = new List<Brinicle>();

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public string UniqueId { get; set; }

        /**
         *
         * Damage Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float Damage { get; set; } 
    }
}
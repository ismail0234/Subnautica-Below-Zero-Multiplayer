namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class CreatureProcessArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.CreatureProcess;

        /**
         *
         * ProcessTime değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public double ProcessTime { get; set; }

        /**
         *
         * CreatureId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ushort CreatureId { get; set; }

        /**
         *
         * CreatureType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public TechType CreatureType { get; set; }

        /**
         *
         * Component değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public NetworkCreatureComponent Component { get; set; }
    }
}
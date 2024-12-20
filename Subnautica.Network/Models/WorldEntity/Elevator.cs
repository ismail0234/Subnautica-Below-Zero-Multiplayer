namespace Subnautica.Network.Models.WorldEntity
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;

#pragma warning disable CS0612

    [MessagePackObject]
    public class Elevator : NetworkWorldEntityComponent
    {
        /**
         *
         * ProcessType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override EntityProcessType ProcessType { get; set; } = EntityProcessType.Elevator;

        /**
         *
         * IsUp değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsUp { get; set; }

        /**
         *
         * StartTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float StartTime { get; set; }
    }
}
namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Thumper : NetworkDynamicEntityComponent
    {
        /**
         *
         * Position Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Charge Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float Charge { get; set; }
    }
}
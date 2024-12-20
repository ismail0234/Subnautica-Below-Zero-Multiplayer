namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared
{
    using MessagePack;

    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class OxygenPipeItem
    {
        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * ParentId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string ParentId { get; set; }

        /**
         *
         * Position Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 Position { get; set; }
    }
}

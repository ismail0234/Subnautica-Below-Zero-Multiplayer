namespace Subnautica.Network.Models.WorldEntity
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class DestroyableDynamicEntity : NetworkWorldEntityComponent
    {
        /**
         *
         * ProcessType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override EntityProcessType ProcessType { get; set; } = EntityProcessType.DestroyableDynamic;

        /**
         *
         * PickupItem değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public WorldPickupItem PickupItem { get; set; }

        /**
         *
         * IsWorldStreamer değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsWorldStreamer { get; set; }
    }
}
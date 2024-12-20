namespace Subnautica.Network.Models.WorldEntity
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class DestroyableEntity : NetworkWorldEntityComponent
    {
        /**
         *
         * ProcessType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override EntityProcessType ProcessType { get; set; } = EntityProcessType.Destroyable;

        /**
         *
         * Health değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float Health { get; set; }
    }
}
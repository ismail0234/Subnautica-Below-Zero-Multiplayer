namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Hoverbike : NetworkDynamicEntityComponent
    {
        /**
         *
         * Modules Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public List<UpgradeConsoleItem> Modules { get; set; } = new List<UpgradeConsoleItem>()
        {
            new UpgradeConsoleItem()
        };

        /**
         *
         * ColorCustomizer Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroColorCustomizer ColorCustomizer { get; set; } = new ZeroColorCustomizer();

        /**
         *
         * Charge Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float Charge { get; set; } = 100f;

        /**
         *
         * IsLightActive Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsLightActive { get; set; } = true;

        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(200f, 200f);
    }
}

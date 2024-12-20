namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class SeaTruck : NetworkDynamicEntityComponent
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
            new UpgradeConsoleItem(),
            new UpgradeConsoleItem(),
            new UpgradeConsoleItem(),
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
         * PowerCells Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public List<PowerCell> PowerCells { get; set; } = new List<PowerCell>()
        {
            new PowerCell(),
            new PowerCell(),
        };

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
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(500f, 500f);

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruck Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {

            foreach (var powerCell in this.PowerCells)
            {
                powerCell.UniqueId = Network.Identifier.GenerateUniqueId();
            }

            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

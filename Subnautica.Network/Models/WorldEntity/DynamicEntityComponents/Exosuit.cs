namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Exosuit : NetworkDynamicEntityComponent
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
            new UpgradeConsoleItem(),
            new UpgradeConsoleItem(),
            new UpgradeConsoleItem()
        };       

        /**
         *
         * PowerCells Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public List<PowerCell> PowerCells { get; set; } = new List<PowerCell>()
        {
            new PowerCell(),
            new PowerCell(),
        };

        /**
         *
         * ColorCustomizer Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroColorCustomizer ColorCustomizer { get; set; } = new ZeroColorCustomizer();

        /**
         *
         * StorageContainer Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public Metadata.StorageContainer StorageContainer { get; set; }

        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(600f, 600f);

        /**
         *
         * Depolama dolabının boyutunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResizeStorageContainer()
        {
            this.StorageContainer.Resize(this.StorageContainer.GetSizeX(), Convert.ToByte(this.Modules.Count(q => q.ModuleType == TechType.VehicleStorageModule) + 4));
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Exosuit Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            this.StorageContainer = Metadata.StorageContainer.Create(6, 4);

            foreach (var powerCell in this.PowerCells)
            {
                powerCell.UniqueId = Network.Identifier.GenerateUniqueId();
            }

            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}
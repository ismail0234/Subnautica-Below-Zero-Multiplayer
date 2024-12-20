namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class SeaTruckStorageModule : NetworkDynamicEntityComponent
    {
        /**
         *
         * Lockers Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public List<SeaTruckLockerItem> Lockers { get; set; } = new List<SeaTruckLockerItem>()
        {
            new SeaTruckLockerItem(),
            new SeaTruckLockerItem(),
            new SeaTruckLockerItem(),
            new SeaTruckLockerItem(),
            new SeaTruckLockerItem()
        };

        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(500f, 500f);

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckStorageModule Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            foreach (var locker in this.Lockers)
            {
                locker.UniqueId = Network.Identifier.GenerateUniqueId();
            }

            this.Lockers.ElementAt(0).StorageContainer = Metadata.StorageContainer.Create(3, 5);
            this.Lockers.ElementAt(1).StorageContainer = Metadata.StorageContainer.Create(6, 3);
            this.Lockers.ElementAt(2).StorageContainer = Metadata.StorageContainer.Create(4, 3);
            this.Lockers.ElementAt(3).StorageContainer = Metadata.StorageContainer.Create(4, 3);
            this.Lockers.ElementAt(4).StorageContainer = Metadata.StorageContainer.Create(3, 5);

            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

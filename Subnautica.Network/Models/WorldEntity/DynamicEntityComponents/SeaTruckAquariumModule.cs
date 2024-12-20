namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class SeaTruckAquariumModule : NetworkDynamicEntityComponent
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
            new SeaTruckLockerItem()
        };

        /**
         *
         * LeftStorageTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public float LeftStorageTime { get; set; }

        /**
         *
         * RightStorageTime Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float RightStorageTime { get; set; }

        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(500f, 500f);

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckAquariumModule Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            foreach (var locker in this.Lockers)
            {
                locker.UniqueId         = Network.Identifier.GenerateUniqueId();
                locker.StorageContainer = Metadata.StorageContainer.Create(2, 4);
            }

            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

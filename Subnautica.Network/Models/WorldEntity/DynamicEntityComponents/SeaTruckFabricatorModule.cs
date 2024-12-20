namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class SeaTruckFabricatorModule : NetworkDynamicEntityComponent
    {
        /**
         *
         * FabricatorUniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string FabricatorUniqueId { get; set; }

        /**
         *
         * Lockers Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public List<SeaTruckLockerItem> Lockers { get; set; } = new List<SeaTruckLockerItem>()
        {
            new SeaTruckLockerItem()
        };

        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(500f, 500f);

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckFabricatorModule Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            this.FabricatorUniqueId = Network.Identifier.GenerateUniqueId();

            foreach (var locker in this.Lockers)
            {
                locker.UniqueId         = Network.Identifier.GenerateUniqueId();
                locker.StorageContainer = Metadata.StorageContainer.Create(6, 2);
            }

            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

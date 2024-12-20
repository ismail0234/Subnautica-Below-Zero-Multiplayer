namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class SeaTruckSleeperModule : NetworkDynamicEntityComponent
    {
        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public LiveMixin LiveMixin { get; set; } = new LiveMixin(500f, 500f);
        
        /**
         *
         * LiveMixin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public BedSideItem Bed { get; set; } = new BedSideItem();

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckSleeperModule Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using System;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class SeaTruckDockingModule : NetworkDynamicEntityComponent
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
         * Vehicle Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public WorldDynamicEntity Vehicle { get; set; }

        /**
         *
         * Aracı modüle kenetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Dock(WorldDynamicEntity entity)
        {
            if (this.IsDocked())
            {
                return false;
            }

            this.Vehicle = entity;
            return true;
        }

        /**
         *
         * Aracın kenetlenmesini kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Undock(out WorldDynamicEntity vehicle)
        {
            vehicle = this.Vehicle;

            if (this.IsDocked())
            {
                this.Vehicle = null;
                return true;
            }

            return false;
        }

        /**
         *
         * Kenetlenme durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDocked()
        {
            return this.Vehicle != null;
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckDockingModule Initialize(Action<NetworkDynamicEntityComponent> onEntityComponentInitialized)
        {
            onEntityComponentInitialized?.Invoke(this);
            return this;
        }
    }
}

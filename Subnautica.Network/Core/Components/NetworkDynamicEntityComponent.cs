namespace Subnautica.Network.Core.Components
{
    using System;

    using MessagePack;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    [Union(0, typeof(WorldEntityModel.SeaTruck))]
    [Union(1, typeof(WorldEntityModel.Exosuit))]
    [Union(2, typeof(WorldEntityModel.Hoverbike))]
    [Union(3, typeof(WorldEntityModel.Constructor))]
    [Union(4, typeof(WorldEntityModel.SmallStorage))]
    [Union(5, typeof(WorldEntityModel.QuantumLocker))]
    [Union(6, typeof(WorldEntityModel.LEDLight))]
    [Union(7, typeof(WorldEntityModel.Thumper))]
    [Union(8, typeof(WorldEntityModel.Flare))]
    [Union(9, typeof(WorldEntityModel.Beacon))]
    [Union(10, typeof(WorldEntityModel.SeaTruckAquariumModule))]
    [Union(11, typeof(WorldEntityModel.SeaTruckDockingModule))]
    [Union(12, typeof(WorldEntityModel.SeaTruckFabricatorModule))]
    [Union(13, typeof(WorldEntityModel.SeaTruckSleeperModule))]
    [Union(14, typeof(WorldEntityModel.SeaTruckStorageModule))]
    [Union(15, typeof(WorldEntityModel.SeaTruckTeleportationModule))]
    [Union(16, typeof(WorldEntityModel.SpyPenguin))]
    [Union(17, typeof(WorldEntityModel.MapRoomCamera))]
    [Union(18, typeof(WorldEntityModel.PipeSurfaceFloater))]
    [Union(19, typeof(WorldEntityModel.WaterParkCreature))]
    [MessagePackObject]
    public abstract class NetworkDynamicEntityComponent
    {
        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponent<T>()
        {
            if (this is T)
            {
                return (T) Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }
}

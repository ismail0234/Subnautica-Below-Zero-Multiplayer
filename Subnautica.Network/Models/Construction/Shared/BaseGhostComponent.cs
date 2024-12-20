namespace Subnautica.Network.Models.Construction.Shared
{
    using System;

    using MessagePack;

    [Union(0, typeof(BaseAddFaceGhostComponent))]
    [Union(1, typeof(BaseAddLadderGhostComponent))]
    [Union(2, typeof(BaseAddBulkheadGhostComponent))]
    [Union(3, typeof(BaseAddPartitionGhostComponent))]
    [Union(4, typeof(BaseAddPartitionDoorGhostComponent))]
    [Union(5, typeof(BaseAddModuleGhostComponent))]
    [Union(6, typeof(BaseAddCellGhostComponent))]
    [Union(7, typeof(BaseAddCorridorGhostComponent))]
    [Union(8, typeof(BaseAddConnectorGhostComponent))]
    [Union(9, typeof(BaseAddMapRoomGhostComponent))]
    [Union(10, typeof(BaseAddWaterParkGhostComponent))]
    [MessagePackObject]
    public abstract class BaseGhostComponent
    {
        /**
         *
         * TargetBaseId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string TargetBaseId { get; set; }

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
                return (T)Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }
}

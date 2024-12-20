namespace Subnautica.Network.Core.Components
{
    using System;

    using MessagePack;

    using Subnautica.API.Enums;

    using WorldEntity = Subnautica.Network.Models.WorldEntity;

    [Union(0, typeof(WorldEntity.RestrictedEntity))]
    [Union(1, typeof(WorldEntity.StaticEntity))]
    [Union(2, typeof(WorldEntity.OxygenPlant))]
    [Union(3, typeof(WorldEntity.SupplyCrate))]
    [Union(4, typeof(WorldEntity.Databox))]
    [Union(5, typeof(WorldEntity.DestroyableEntity))]
    [Union(6, typeof(WorldEntity.PlantEntity))]
    [Union(7, typeof(WorldEntity.BulkheadDoor))]
    [Union(8, typeof(WorldEntity.SealedObject))]
    [Union(9, typeof(WorldEntity.Elevator))]
    [Union(10, typeof(WorldEntity.Drillable))]
    [Union(11, typeof(WorldEntity.DestroyableDynamicEntity))]
    [MessagePackObject]
    public abstract class NetworkWorldEntityComponent
    {
        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * IsSpawnable değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsSpawnable { get; set; } = true;

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public virtual EntityProcessType ProcessType { get; set; } = EntityProcessType.None;

        /**
         *
         * Spawn durumunu pasif yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableSpawn()
        {
            this.IsSpawnable = false;
        }

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

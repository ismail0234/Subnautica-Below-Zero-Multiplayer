namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    public class FruitHarvestingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public FruitHarvestingEventArgs(PickPrefab pickPrefab, string uniqueId, TechType techType, byte maxSpawnableFruit, float spawnInterval, bool isAllowed = true)
        {
            this.PickPrefab          = pickPrefab;
            this.UniqueId            = uniqueId;
            this.TechType            = techType;
            this.MaxSpawnableFruit   = maxSpawnableFruit;
            this.SpawnInterval       = spawnInterval;
            this.IsAllowed           = isAllowed;
            this.IsStaticWorldEntity = Network.StaticEntity.IsStaticEntity(uniqueId);
        }

        /**
         *
         * PickPrefab değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PickPrefab PickPrefab { get; set; }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * MaxSpawnableFruit değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte MaxSpawnableFruit { get; set; }

        /**
         *
         * SpawnInterval değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float SpawnInterval { get; set; }

        /**
         *
         * IsStaticWorldEntity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticWorldEntity { get; set; }

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
namespace Subnautica.Network.Models.Storage.World
{
    using System;
    using System.Collections.Generic;

    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldStreamer;

    using WorldChildrens = Subnautica.Network.Models.Storage.World.Childrens;

    [MessagePackObject]
    [Serializable]
    public class World
    {
        /**
         *
         * Sunucu saatini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public double ServerTime { get; set; } = 400f;

        /**
         *
         * En Son Yapı id numarasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public uint LastConstructionId { get; set; } = 0;

        /**
         *
         * IsFirstLogin Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsFirstLogin { get; set; } = true;

        /**
         *
         * Sunucu'daki yapıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public byte[] Constructions { get; set; }

        /**
         *
         * Açılan şarkıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public List<string> JukeboxDisks { get; set; } = new List<string>();

        /**
         *
         * Güç kaynaklarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public HashSet<WorldChildrens.PowerSource> PowerSources { get; set; } = new HashSet<WorldChildrens.PowerSource>();

        /**
         *
         * Kalıcı Dünya Nesneleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public Dictionary<string, NetworkWorldEntityComponent> PersistentEntities { get; set; } = new Dictionary<string, NetworkWorldEntityComponent>();

        /**
         *
         * Dünya Nesneleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public HashSet<WorldDynamicEntity> DynamicEntities { get; set; } = new HashSet<WorldDynamicEntity>();

        /**
         *
         * Dünya Hızı.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float WorldSpeed { get; set; } = 1f;

        /**
         *
         * Son Uyku Zamanı.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public float TimeLastSleep { get; set; } = 0f;

        /**
         *
         * Zaman atlama modu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public bool SkipTimeMode { get; set; } = false;

        /**
         *
         * Zaman atlama bitiş zamanı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public float SkipModeEndTime { get; set; } = 0f;

        /**
         *
         * SupplyDrops Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public List<SupplyDrop> SupplyDrops { get; set; } = new List<SupplyDrop>();

        /**
         *
         * Bases Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public List<Base> Bases { get; set; } = new List<Base>();

        /**
         *
         * LastItemId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public ushort LastItemId { get; set; }

        /**
         *
         * QuantumLocker Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public Metadata.StorageContainer QuantumLocker { get; set; } = Metadata.StorageContainer.Create(4, 4);

        /**
         *
         * SeaTruckConnections Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(16)]
        public Dictionary<string, string> SeaTruckConnections { get; set; } = new Dictionary<string, string>();

        /**
         *
         * SpawnPoints Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(17)]
        public HashSet<ZeroSpawnPointSimple> SpawnPoints { get; set; } = new HashSet<ZeroSpawnPointSimple>();

        /**
         *
         * IsWorldGenerated Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(18)]
        public bool IsWorldGenerated { get; set; } = false;

        /**
         *
         * ActivatedPrecursorTeleporters Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(19)]
        public List<string> ActivatedPrecursorTeleporters { get; set; } = new List<string>();

        /**
         *
         * Brinicles Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(20)]
        public HashSet<Brinicle> Brinicles { get; set; } = new HashSet<Brinicle>();

        /**
         *
         * Brinicles Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(21)]
        public HashSet<CosmeticItem> CosmeticItems { get; set; } = new HashSet<CosmeticItem>();

        /**
         *
         * DiscoveredTechTypes Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(22)]
        public HashSet<TechType> DiscoveredTechTypes { get; set; } = new HashSet<TechType>();
    }
}
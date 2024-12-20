namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Oculus.Platform;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Core;
    using Subnautica.Network.Models.WorldStreamer;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using UWE;

    using static EntitySlot;

    public class WorldStreamer
    {
        /**
         *
         * Hücre boyutu (Kullanımdan kaldırıldı)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const int CellSize = 64;

        /**
         *
         * Dünyadaki spawn noktalarının hepsini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroSpawnPointContainer SpawnPointContainer { get; set; } = null;

        /**
         *
         * Yumurtlama ihtimali olan nesneleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private LootDistributionData LootDistribution { get; set; }

        /**
         *
         * Yumurtlama ihtimali olan nesneleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<CSVEntitySpawner.Data> SpawnerData = new List<CSVEntitySpawner.Data>();

        /**
         *
         * Slotları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<int, ZeroSpawnPoint> SpawnPoints { get; set; } = new Dictionary<int, ZeroSpawnPoint>(100000);

        /**
         *
         * Gezegen oluşturulurken Fragment detaylarını içerir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<TechType, int> GeneratorFragmentData = new Dictionary<TechType, int>();

        /**
         *
         * Max ortaya çıkabilecek parçalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<TechType, int> FragmentsMax = new Dictionary<TechType, int>()
        {
            { TechType.LaserCutterFragment  , 15 },
            { TechType.ThermalPlantFragment , 15 },
            { TechType.ExosuitTorpedoArmFragment, 10 },
            { TechType.ExosuitPropulsionArmFragment, 10 },
            { TechType.SeaglideFragment, 22 },
            { TechType.SeaTruckFragment, 10 },
            { TechType.ConstructorFragment, 22 },
            { TechType.SeaTruckAquariumModuleFragment, 10 },
            { TechType.GravSphereFragment, 10 },
        };


        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Initialize(HashSet<ZeroSpawnPointSimple> spawnPoints, bool isSpawnPointExists)
        {
            this.SpawnPoints.Clear();

            try
            {
                Directory.CreateDirectory(Paths.GetMultiplayerClientSavePath(Network.Session.Current.ServerId));

                var localSpawnPointPath = Paths.GetMultiplayerClientSpawnPointPath(Network.Session.Current.ServerId);

                if (!isSpawnPointExists && spawnPoints != null)
                {
                    File.WriteAllBytes(localSpawnPointPath, NetworkTools.Serialize(spawnPoints));
                }

                if (!File.Exists(localSpawnPointPath))
                {
                    return false;
                }

                foreach (var spawnPoint in NetworkTools.Deserialize<HashSet<ZeroSpawnPointSimple>>(File.ReadAllBytes(localSpawnPointPath)))
                {
                    this.SpawnPoints[spawnPoint.SlotId] = this.CopyDataFromContainer(spawnPoint.ConvertToZeroSpawnPoint());

                    if (WorldEntityDatabase.TryGetInfo(spawnPoint.ClassId, out var info))
                    {
                        this.SpawnPoints[spawnPoint.SlotId].TechType = info.techType;
                    }
                }

                foreach (var slot in spawnPoints)
                {
                    this.SpawnPoints[slot.SlotId].NextRespawnTime = slot.NextRespawnTime;
                    this.SpawnPoints[slot.SlotId].Health          = slot.Health;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"WorldStreamer.Client.Exception: {ex}");
            }

            return true;
        }

        /**
         *
         * Yerel dosyadaki spawn point miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetClientSpawnPointCount()
        {
            try
            {
                var localSpawnPointPath = Paths.GetMultiplayerClientSpawnPointPath(Network.Session.Current.ServerId);
                if (!File.Exists(localSpawnPointPath))
                {
                    return 0;
                }

                return NetworkTools.Deserialize<HashSet<ZeroSpawnPointSimple>>(File.ReadAllBytes(localSpawnPointPath)).Count;
            }
            catch (System.Exception ex)
            {
                Log.Error($"WorldStreamer.GetLocalSpawnPointCount.Exception: {ex}, Id: " + Network.Session.Current.ServerId);
            }

            return 0;
        }

        /**
         *
         * Dünyayı oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void GenerateWorld()
        {
            var timing = new Stopwatch();
            timing.Start();

            if (LargeWorld.main.streamer.cellManager.spawner is CSVEntitySpawner spawner)
            {
                this.LootDistribution = spawner.lootDistribution;
            }
            else
            {
                Log.Error("LootDistribution Is Null");
            }

            this.GeneratorFragmentData.Clear();
            this.SpawnPoints.Clear();

            foreach (var slot in this.SpawnPointContainer.SpawnPoints)
            {
                var result = this.GetPrefabForSlot(slot.Value);
                if (result.count > 0)
                {
                    this.SpawnPoints[slot.Value.SlotId] = slot.Value.Clone();

                    if (WorldEntityDatabase.TryGetInfo(result.classId, out var info))
                    {
                        this.SpawnPoints[slot.Value.SlotId].SetActive(true, info.techType, info.classId);
                    }
                }
            }

            timing.Stop();

            Log.Info("World Creation Time: " + timing.ElapsedMilliseconds);
        }

        /**
         *
         * Yumurtlama noktaları önbelleğe alındı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSpawnPointContainerInitialized()
        {
            return this.SpawnPointContainer != null;
        }

        /**
         *
         * Yumurtlama nokta kapsayıcısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CreateSpawnPointContainer()
        {
            System.Threading.Tasks.Task.Run(() => {
                var spawnPoints = Paths.GetLauncherGameCorePath("SpawnPoints.bin");
                if (File.Exists(spawnPoints))
                {
                    this.SpawnPointContainer = NetworkTools.Deserialize<ZeroSpawnPointContainer>(File.ReadAllBytes(spawnPoints));
                }
            });
        }

        /**
         *
         * Yumurtlama nokta kapsayıcısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroSpawnPoint CopyDataFromContainer(ZeroSpawnPoint spawnPoint)
        {
            if (this.SpawnPointContainer.SpawnPoints.TryGetValue(spawnPoint.SlotId, out var temp))
            {
                spawnPoint.LeashPosition = temp.LeashPosition;
                spawnPoint.LeashRotation = temp.LeashRotation;
                return spawnPoint;
            }

            return spawnPoint;
        }

        /**
         *
         * Slotu pasif hale getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool DisableSlot(int slotId, float nextRespawnTime = -1)
        {
            var slot = this.GetSlotById(slotId);
            if (slot == null)
            {
                return false;
            }

            slot.NextRespawnTime = nextRespawnTime;

            var gameObject = Network.Identifier.GetGameObject(slotId.ToWorldStreamerId(), true);
            if (gameObject)
            {
                gameObject.SetActive(false);
            }

            return true;
        }

        /**
         *
         * Slotu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPoint GetSlotById(int slotId)
        {
            if (this.SpawnPoints.TryGetValue(slotId, out var spawnPoint))
            {
                return spawnPoint;
            }

            return null;
        }

        /**
         *
         * Aktif spawn noktalarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<int, ZeroSpawnPoint> GetSpawnPoints()
        {
            return this.SpawnPoints;
        }

        /**
         *
         * Slotun barındıracağı nesne bilgisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Filler GetPrefabForSlot(ZeroSpawnPoint slot, bool filterKnown = true)
        {
            if (slot.Density == -1f)
            {
                var entity = WorldEntityDatabase.main.infos.FirstOrDefault(q => q.Value.techType == (TechType) slot.BiomeType);

                return new EntitySlot.Filler()
                {
                    count   = 1,
                    classId = entity.Value.classId
                };
            }

            if (!this.LootDistribution.GetBiomeLoot(slot.BiomeType, out var biome))
            {
                return new EntitySlot.Filler();
            }

            if (SpawnerData.Count > 0)
            {
                SpawnerData.Clear();
            }

            float fragmentProbability = 0f, completedFragmentProbability = 0f;

            foreach (var prefab in biome.prefabs)
            {
                if (string.Equals(prefab.classId, "None") || !this.LootDistribution.srcDistribution.TryGetValue(prefab.classId, out var _))
                {
                    continue;
                }

                if (!WorldEntityDatabase.TryGetInfo(prefab.classId, out var info))
                {
                    continue;
                }

                if (!slot.IsTypeAllowed(info.slotType))
                {
                    continue;
                }

                var avgProbability = prefab.probability / slot.GetDensity();
                if (avgProbability <= 0.0f)
                {
                    continue;
                }

                var isFragment = false;
                if (filterKnown)
                {
                    isFragment = PDAScanner.IsFragment(info.techType);
                }

                SpawnerData.Add(new CSVEntitySpawner.Data()
                {
                    classId     = prefab.classId,
                    count       = 1,
                    probability = avgProbability,
                    isFragment  = isFragment
                });

                if (isFragment)
                {
                    fragmentProbability += avgProbability;
                }
            }

            if (SpawnerData.Count <= 0)
            {
                return new EntitySlot.Filler();
            }

            var fragmentMultiplier = 1f;
            var isFragmentMultiplier = completedFragmentProbability > 0.0f && fragmentProbability > 0.0f;
            if (isFragmentMultiplier)
            {
                fragmentMultiplier = (completedFragmentProbability + fragmentProbability) / fragmentProbability;
            }

            var totalProbability = 0f;
            for (int i = 0; i < SpawnerData.Count; i++)
            {
                var spawner = SpawnerData[i];
                if (isFragmentMultiplier && spawner.isFragment)
                {
                    spawner.probability *= fragmentMultiplier;
                    SpawnerData[i] = spawner;
                }

                totalProbability += spawner.probability;
            }

            CSVEntitySpawner.Data result;
            result.count = 0;
            result.classId = null;

            if (totalProbability > 0.0f)
            {
                var randomValue = UnityEngine.Random.value;
                if (totalProbability > 1.0f)
                {
                    randomValue *= totalProbability;
                }

                var currentProbability = 0.0f;
                foreach (var spawner in SpawnerData)
                {
                    currentProbability += spawner.probability;
                    if (currentProbability >= randomValue)
                    {
                        result = spawner;
                        break;
                    }
                }
            }

            SpawnerData.Clear();

            var filler = new EntitySlot.Filler();

            if (result.count > 0)
            {
                if (WorldEntityDatabase.TryGetInfo(result.classId, out var info) && info.techType.IsFragment())
                {
                    if (this.GeneratorFragmentData.TryGetValue(info.techType, out var totalFragment))
                    {
                        this.FragmentsMax.TryGetValue(info.techType, out var fragmentMaxCount);

                        if (fragmentMaxCount <= 0)
                        {
                            fragmentMaxCount = 5;
                        }

                        if (totalFragment > fragmentMaxCount)
                        {
                            return filler;
                        }

                        this.GeneratorFragmentData[info.techType]++;
                    }
                    else
                    {
                        this.GeneratorFragmentData[info.techType] = 1;
                    }
                }
 
                filler.classId = result.classId;
                filler.count   = result.count;
            }

            return filler;
        }

        /**
         *
         * Hücrenin yüklenip yüklenmediğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCellLoaded(Subnautica.Network.Structures.ZeroVector3 position)
        {
            return global::LargeWorldStreamer.main.IsRangeActiveAndBuilt(new UnityEngine.Bounds(position.ToVector3(), Vector3.zero));
        }

        /**
         *
         * Tüm verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.SpawnPointContainer = null;
            this.LootDistribution    = null;
            this.SpawnPoints.Clear();
            this.GeneratorFragmentData.Clear();
            
            SpawnerData.Clear();
        }
    }
}
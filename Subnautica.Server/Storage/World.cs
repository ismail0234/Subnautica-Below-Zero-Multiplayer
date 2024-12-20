namespace Subnautica.Server.Storage
{
    using System;
    using System.IO;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Extensions;
    using Subnautica.Server.Logic.Furnitures;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using WorldStorage     = Subnautica.Network.Models.Storage.World;
    using WorldChildrens   = Subnautica.Network.Models.Storage.World.Childrens;

    public class World : BaseStorage
    {
        /**
         *
         * Dünya sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldStorage.World Storage { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Start(string serverId)
        {
            this.ServerId = serverId;
            this.FilePath = Paths.GetMultiplayerServerSavePath(this.ServerId, "World.bin");
            this.InitializePath();
            this.Load();
        }

        /**
         *
         * Sunucu dünya verilerini belleğe yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Load()
        {
            if (File.Exists(this.FilePath))
            {
                lock (this.ProcessLock)
                {
                    try
                    {
                        this.Storage = NetworkTools.Deserialize<WorldStorage.World>(File.ReadAllBytes(this.FilePath));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"World.Load: {e}");
                    }
                }
            }
            else
            {
                this.Storage = new WorldStorage.World();
                this.SaveToDisk();
            }

            if (Core.Server.DEBUG)
            {
                Log.Info("World Detail: ");
                Log.Info("---------------------------------------------------------------");
                Log.Info(String.Format("ServerTime         : {0}", this.Storage.ServerTime));
                Log.Info("---------------------------------------------------------------");
            }
        }

        /**
         *
         * Verileri diske yazar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void SaveToDisk()
        {
            lock (this.ProcessLock)
            {
                this.WriteToDisk(this.Storage);
            }
        }

        /**
         *
         * Supply drop nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetSupplyDrop(out WorldChildrens.SupplyDrop supplyDrop)
        {
            supplyDrop = this.Storage.SupplyDrops.Where(q => q.Key == API.Constants.SupplyDrop.Lifepod).FirstOrDefault();
            return supplyDrop != null;
        }

        /**
         *
         * SeaTruck bağlantısı ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddSeaTruckConnection(string frontModuleId, string backModuleId, bool checkBackModule = true)
        {
            var frontModule = Core.Server.Instance.Storages.World.GetDynamicEntity(frontModuleId);
            if (frontModule == null || !frontModule.TechType.IsSeaTruckModule(true))
            {
                return false;
            }

            if (checkBackModule)
            {
                var backModule = Core.Server.Instance.Storages.World.GetDynamicEntity(backModuleId);
                if (backModule == null || !backModule.TechType.IsSeaTruckModule(true))
                {
                    return false;
                }
            }

            lock (this.ProcessLock)
            {
                if (this.Storage.SeaTruckConnections.ContainsKey(frontModuleId))
                {
                    return false;
                }

                if (this.Storage.SeaTruckConnections.Any(q => q.Value == backModuleId))
                {
                    return false;
                }

                // FrontModule Id -> Ön tarafı bağlanır. (Arkadaki Modülün önü bağlanır)
                // BackModule  Id -> Arka tarafı bağlanır. (Öndeki Modülün arkasına bağlanır)

                frontModule.SetParent(backModuleId);

                Core.Server.Instance.Logices.EntityWatcher.RemoveWatcherByEntity(frontModule);

                this.Storage.SeaTruckConnections[frontModuleId] = backModuleId;
                return true;
            }
        }

        /**
         *
         * SeaTruck bağlantısı koprarır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string RemoveSeaTruckConnection(string frontModuleId, bool checkModule = true)
        {
            if (checkModule)
            {
                var frontModule = Core.Server.Instance.Storages.World.GetDynamicEntity(frontModuleId);
                if (frontModule == null)
                {
                    return null;
                }
            }

            var connection = this.Storage.SeaTruckConnections.FirstOrDefault(q => q.Value == frontModuleId);
            if (connection.Value == null)
            {
                return null;
            }

            this.Storage.SeaTruckConnections.Remove(connection.Key);

            var backModule = Core.Server.Instance.Storages.World.GetDynamicEntity(connection.Key);
            if (backModule == null)
            {
                return null;
            }

            backModule.SetParent(null);
            return connection.Key;
        }

        /**
         *
         * Üs verisini döner yoksa ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetBase(string baseId, out Base baseComponent)
        {
            lock (this.ProcessLock)
            {
                var baseData = this.Storage.Bases.Where(q => q.BaseId == baseId).FirstOrDefault();
                if (baseData != null)
                {
                    baseComponent = baseData;
                }
                else
                {
                    baseComponent = new Base()
                    {
                        BaseId = baseId
                    };

                    this.Storage.Bases.Add(baseComponent);
                }
                
                return true;
            }
        }

        /**
         *
         * Üs verisini siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveBase(string baseId)
        {
            lock (this.ProcessLock)
            {
                this.Storage.Bases.RemoveAll(q => q.BaseId == baseId);
            }
        }

        /**
         *
         * Üs verisini siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveBase(Base baseComp)
        {
            lock (this.ProcessLock)
            {
                this.Storage.Bases.Remove(baseComp);
            }
        }

        /**
         *
         * Öncül ışınlanma portalını aktif eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ActivateTeleportPortal(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.ActivatedPrecursorTeleporters.Contains(uniqueId))
                {
                    return false;
                }

                this.Storage.ActivatedPrecursorTeleporters.Add(uniqueId);
                return true;
            }
        }

        /**
         *
         * Yapıları günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateConstructions(byte[] constructions)
        {
            lock (this.ProcessLock)
            {
                this.Storage.Constructions = constructions;
                return true;
            }
        }

        /**
         *
         * Dünyada nesne ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddWorldDynamicEntity(WorldDynamicEntity entity)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.DynamicEntities.Any(q => q.UniqueId == entity.UniqueId))
                {
                    return false;
                }

                this.Storage.DynamicEntities.Add(entity);
                return true;
            }
        }

        /**
         *
         * Dünyadaki nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity GetDynamicEntity(string uniqueId)
        {
            if (uniqueId.IsNull())
            {
                return null;
            }

            lock (this.ProcessLock)
            {
                return this.Storage.DynamicEntities.FirstOrDefault(q => q.UniqueId == uniqueId);
            }
        }

        /**
         *
         * Araç döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity GetVehicle(string uniqueId, bool ignoreMoonpool = false)
        {
            lock (this.ProcessLock)
            {
                var vehicle = this.Storage.DynamicEntities.FirstOrDefault(q => q.UniqueId == uniqueId);

                foreach (var item in Server.Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMoonpool || q.Value.TechType == TechType.BaseMoonpoolExpansion))
                {
                    var moonpool = item.Value.EnsureComponent<Metadata.BaseMoonpool>();
                    if (moonpool.IsDocked && moonpool.Vehicle.UniqueId == uniqueId)
                    {
                        if (ignoreMoonpool)
                        {
                            return null;
                        }

                        return moonpool.Vehicle;
                    }
                    else if (moonpool.ExpansionManager.IsTailDocked() && ignoreMoonpool && vehicle != null)
                    {
                        if (vehicle.UniqueId == moonpool.ExpansionManager.TailId)
                        {
                            return null;
                        }

                        foreach (var fontModule in vehicle.GetSeaTruckFrontModule())
                        {
                            if (fontModule != null && fontModule.UniqueId == moonpool.ExpansionManager.TailId)
                            {
                                return null;
                            }
                        }
                    }
                }

                if (vehicle != null)
                {
                    return vehicle;
                }

                foreach (var item in this.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckDockingModule))
                {
                    var component = item.Component.GetComponent<WorldEntityModel.SeaTruckDockingModule>();
                    if (component.IsDocked() && component.Vehicle.UniqueId == uniqueId)
                    {
                        return component.Vehicle;
                    }
                }

                return null;
            }
        }

        /**
         *
         * Dünya üzerinde kalıcı kozmetik kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveCosmeticItem(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.CosmeticItems.RemoveWhere(q => q.StorageItem.ItemId == uniqueId) > 0;
            }
        }

        /**
         *
         * Cosmetic Item nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CosmeticItem GetCosmeticItem(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.CosmeticItems.FirstOrDefault(q => q.StorageItem.ItemId == uniqueId);
            }
        }

        /**
         *
         * Dünya üzerinde kalıcı kozmetik eşyası ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddCosmeticItem(string uniqueId, string baseId, TechType techType, ZeroVector3 position, ZeroQuaternion rotation)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.CosmeticItems.Any(q => q.StorageItem.ItemId == uniqueId))
                {
                    return false;
                }

                return this.Storage.CosmeticItems.Add(new CosmeticItem(StorageItem.Create(uniqueId, techType), baseId, position, rotation));
            }
        }

        /**
         *
         * Dünya kalıcı nesnesini bileşen olarak döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetDynamicEntityComponent<T>(string uniqueId)
        {
            var entity = this.GetDynamicEntity(uniqueId);
            if (entity == null || entity.Component == null)
            {
                return default;
            }

            return entity.Component.GetComponent<T>();
        }

        /**
         *
         * Dünyadaki nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity GetDynamicEntity(ushort id)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.DynamicEntities.FirstOrDefault(q => q.Id == id);
            }
        }

        /**
         *
         * Dünyadaki nesneyi kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveDynamicEntity(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.DynamicEntities.RemoveWhere(q => q.UniqueId == uniqueId) > 0;
            }
        }

        /**
         *
         * Music diskini ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddDiscoveredResource(TechType techType)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.DiscoveredTechTypes.Add(techType);
            }
        }

        /**
         *
         * Music diskini ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddJukeboxDisk(string trackFile)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.JukeboxDisks.Contains(trackFile))
                {
                    return false;
                }

                this.Storage.JukeboxDisks.Add(trackFile);

                Core.Server.Instance.Logices.Jukebox.SortPlaylist();
                return true;
            }
        }

        /**
         *
         * Dünya kalıcı nesnesini düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SetPersistentEntity(NetworkWorldEntityComponent entity)
        {
            lock (this.ProcessLock)
            {
                if (entity == null || entity.UniqueId.IsNull())
                {
                    return false;
                }

                this.Storage.PersistentEntities[entity.UniqueId] = entity;
                return true;
            }
        }

        /**
         *
         * Dünya kalıcı nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetworkWorldEntityComponent GetPersistentEntity(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.PersistentEntities.TryGetValue(uniqueId, out var entity))
                {
                    return entity;
                }

                return null;
            }
        }

        /**
         *
         * Dünya kalıcı nesnesini bileşen olarak döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetPersistentEntity<T>(string uniqueId)
        {
            var entity = this.GetPersistentEntity(uniqueId);
            if (entity == null)
            {
                return default;
            }

            return entity.GetComponent<T>();
        }

        /**
         *
         * Dünya kalıcı nesne varlığını kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPersistentEntityExists(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                return this.Storage.PersistentEntities.ContainsKey(uniqueId);
            }
        }

        /**
         *
         * Dünya kalıcı pasif nesne ekler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddDisablePersistentEntity(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                if (this.Storage.PersistentEntities.TryGetValue(uniqueId, out var component))
                {
                    if (component.IsSpawnable)
                    {
                        component.DisableSpawn();
                        return true;
                    }

                    return false;
                }

                var entity = new StaticEntity()
                {
                    UniqueId = uniqueId
                };

                entity.DisableSpawn();

                this.SetPersistentEntity(entity);
                return true;
            }
        }

        /**
         *
         * Slotu pasif hale getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool DisableSlot(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                var spawnPoint = this.Storage.SpawnPoints.FirstOrDefault(q => q.SlotId == uniqueId.WorldStreamerToSlotId());
                if (spawnPoint == null)
                {
                    return false;
                }

                var currentTime = Server.Core.Server.Instance.Logices.World.GetServerTime();
                if (!spawnPoint.IsRespawnable(currentTime))
                {
                    return false;
                }

                spawnPoint.NextRespawnTime = spawnPoint.GetNextRespawnTime(currentTime);
                return true;
            }
        }

        /**
         *
         * Sonraki doğma zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetSlotNextRespawnTime(string uniqueId)
        {
            lock (this.ProcessLock)
            {
                var spawnPoint = this.Storage.SpawnPoints.FirstOrDefault(q => q.SlotId == uniqueId.WorldStreamerToSlotId());
                if (spawnPoint == null)
                {
                    return -1f;
                }

                return spawnPoint.NextRespawnTime;
            }
        }
    }
}
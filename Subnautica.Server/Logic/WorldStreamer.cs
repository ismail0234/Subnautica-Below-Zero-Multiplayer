namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.WorldStreamer;
    using Subnautica.Server.Abstracts;

    public class WorldStreamer : BaseLogic
    {
        /**
         *
         * Dünya oluşturma başladı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsWorldGenerateStarting { get; set; } = false;

        /**
         *
         * Dünya oluşturuldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool _IsGeneratedWorld = false;

        /**
         *
         * Dünya oluşturma başladı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<int, ZeroSpawnPointSimple> SpawnPoints { get; set; } = new Dictionary<int, ZeroSpawnPointSimple>();

        /**
         *
         * Varlık dağıtımı yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityDistributionLoaded()
        {
            if (!Core.Server.Instance.Storages.World.Storage.IsWorldGenerated)
            {
                this.IsWorldGenerateStarting = true;
                this.OnUnscaledFixedUpdate(0f);
            }
        }

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUnscaledFixedUpdate(float fixedDeltaTime)
        {
            if (this.IsWorldGenerateStarting && Network.WorldStreamer.IsSpawnPointContainerInitialized())
            {
                this.IsWorldGenerateStarting = false;

                Task.Run(this.GenerateWorld);
            }
            else if (Core.Server.Instance.Storages.World.Storage.IsWorldGenerated && !this._IsGeneratedWorld && Network.WorldStreamer.IsSpawnPointContainerInitialized())
            {
                this.OnWorldGenerated();
            }
        }

        /**
         *
         * Dünyayı oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void GenerateWorld()
        {
            Network.WorldStreamer.GenerateWorld();

            foreach (var spawnPoint in Network.WorldStreamer.GetSpawnPoints())
            {
                Core.Server.Instance.Storages.World.Storage.SpawnPoints.Add(new ZeroSpawnPointSimple(spawnPoint.Value.SlotId, spawnPoint.Value.ClassId, 0));
            }

            this.OnWorldGenerated();

            Core.Server.Instance.Storages.World.Storage.IsWorldGenerated = true;
        }

        /**
         *
         * Dünya oluşturulunca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnWorldGenerated()
        {
            this._IsGeneratedWorld = true;
            
            this.SpawnPoints.Clear();

            var currentTime = Core.Server.Instance.Logices.World.GetServerTime();

            foreach (var spawnPoint in Core.Server.Instance.Storages.World.Storage.SpawnPoints)
            {
                if (UWE.WorldEntityDatabase.TryGetInfo(spawnPoint.ClassId, out var info))
                {
                    spawnPoint.TechType = info.techType;
                }

                if (spawnPoint.NextRespawnTime != 0f && spawnPoint.IsRespawnable(currentTime))
                {
                    spawnPoint.NextRespawnTime = 0f;
                }

                if (spawnPoint.Health != -1f)
                {
                    if (!spawnPoint.TechType.IsDrillable())
                    {
                        spawnPoint.SetHealth(-1f);
                    }  
                }

                this.SpawnPoints.Add(spawnPoint.SlotId, spawnPoint);
            }
        }

        /**
         *
         * Dünya oluşturuldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsGeneratedWorld()
        {
            return this._IsGeneratedWorld && Core.Server.Instance.Storages.World.Storage.IsWorldGenerated;
        }

        /**
         *
         * Spawn point miktarı oluşturuldu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetSpawnPointCount()
        {
            return this.SpawnPoints.Count;
        }

        /**
         *
         * Slotu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPointSimple GetSpawnPointById(int slotId)
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
        public Dictionary<int, ZeroSpawnPointSimple> GetSpawnPoints()
        {
            return this.SpawnPoints;
        }
    }
}
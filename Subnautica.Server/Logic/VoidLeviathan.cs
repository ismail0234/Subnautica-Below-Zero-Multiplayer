namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures.Datas;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    using UnityEngine;

    public class VoidLeviathan : BaseLogic
    {
        /**
         *
         * Zamanlayıcıyı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private StopwatchItem Timing { get; set; } = new StopwatchItem(2000f);

        /**
         *
         * SpawnedCreatures değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private HashSet<ushort> SpawnedCreatures { get; set; } = new HashSet<ushort>();

        /**
         *
         * Oyuncu zamanlarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, double> PlayerTimes { get; set; } = new Dictionary<byte, double>();

        /**
         *
         * ScanRange değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private BaseCreatureData Data { get; set; }

        /**
         *
         * MaxSpawn değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int MaxSpawn { get; set; } = 3;

        /**
         *
         * Yaratık spawnlandı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsCreatureSpawned { get; set; }

        /**
         *
         * Spawner değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private VoidLeviathansSpawner Spawner
        {
            get
            {
                return VoidLeviathansSpawner.main;
            }
        }

        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.Data = TechType.GhostLeviathan.GetCreatureData();
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished())
            {
                this.Timing.Restart();

                if (this.IsLoaded())
                { 
                    this.RemoveVoidLeviathans();
                    this.SpawnVoidLeviathans();

                    if (this.IsCreatureSpawned)
                    {
                        this.IsCreatureSpawned = false;

                        Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger();
                    }
                }
            }
        }

        /**
         *
         * Oyuncuya çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerDisconnected(AuthorizationProfile player)
        {
            this.PlayerTimes[player.PlayerId] = 0;
        }

        /**
         *
         * Sahibi olmayan yaratıkları kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void RemoveVoidLeviathans()
        {
            foreach (var creatureId in this.SpawnedCreatures.ToList())
            {
                if (Server.Instance.Logices.CreatureWatcher.TryGetCreature(creatureId, out var creature))
                {
                    if (creature.IsBusy() || creature.IsExistsOwnership())
                    {
                        continue;
                    }

                    Server.Instance.Logices.CreatureWatcher.UnRegisterCreature(creatureId);

                    this.SpawnedCreatures.Remove(creatureId);
                }
            }
        }

        /**
         *
         * Boşluktaki yaratıkları spawnlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SpawnVoidLeviathans()
        {
            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected)
                {
                    player.SetInVoidBiome(this.Spawner.IsVoidBiome(player.GetBiome()));

                    if (player.IsInVoidBiome)
                    {
                        if (this.PlayerTimes.TryGetValue(player.PlayerId, out var nextTime) == false || nextTime == 0)
                        {
                            this.PlayerTimes[player.PlayerId] = this.CalculateTimeNextSpawn(true);
                        }

                        if (Server.Instance.Logices.World.GetServerTime() >= this.PlayerTimes[player.PlayerId])
                        {
                            var creatureCount = this.GetNearestCreatureCount(player.Position);
                            if (creatureCount < this.MaxSpawn)
                            {
                                this.SpawnCreature(player);
                            }
                            else
                            {
                                this.PlayerTimes[player.PlayerId] = this.CalculateTimeNextSpawn();
                            }
                        }
                    }
                    else 
                    {
                        this.PlayerTimes[player.PlayerId] = 0;
                    }
                }
            }
        }

        /**
         *
         * Yaratığı spawnlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SpawnCreature(AuthorizationProfile player)
        {
            if (this.TryGetSpawnPosition(player.Position.ToVector3(), out var spawnPosition))
            {
                this.IsCreatureSpawned = true;
                this.PlayerTimes[player.PlayerId] = this.CalculateTimeNextSpawn();
                this.SpawnedCreatures.Add(Server.Instance.Logices.CreatureWatcher.RegisterCreature(TechType.GhostLeviathan, spawnPosition.ToZeroVector3(), new ZeroQuaternion()));
            }
        }

        /**
         *
         * Yaratık en yakın spawn konumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool TryGetSpawnPosition(Vector3 playerPosition, out Vector3 spawnPosition)
        {
            spawnPosition = Vector3.zero;

            for (int i = 0; i < 10; i++)
            {
                spawnPosition = playerPosition + UnityEngine.Random.onUnitSphere * (this.Data.VisibilityDistance * 0.9f);

                if (spawnPosition.y < -100f && this.Spawner.IsVoidBiome(LargeWorld.main.GetBiome(spawnPosition)))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Sonraki spawnlanma zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private double CalculateTimeNextSpawn(bool first = false)
        {
            var currentTime = Server.Instance.Logices.World.GetServerTime();
            return first ? currentTime + this.Spawner.timeBeforeFirstSpawn : currentTime + this.Spawner.spawnInterval;
        }

        /**
         *
         * Konum yakınındaki leviathan sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int GetNearestCreatureCount(ZeroVector3 playerPosition)
        {
            int count = 0;

            foreach (var creatureId in this.SpawnedCreatures)
            {
                if (Server.Instance.Logices.CreatureWatcher.TryGetCreature(creatureId, out var creature))
                {
                    if (creature.Position.Distance(playerPosition) < this.Data.VisibilityDistance * this.Data.VisibilityDistance)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /**
         *
         * Yüklenme durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsLoaded()
        {
            return this.Spawner;
        }
    }
}
namespace Subnautica.API.Features.Creatures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Creatures;

    using UnityEngine;

    using UWE;

    using ServerModel = Subnautica.Network.Models.Server;

    public class MultiplayerCreatureManager
    {
        /**
         *
         * Havuz önbellek verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<TechType, List<MultiplayerCreature>> CreaturePools { get; set; } = new Dictionary<TechType, List<MultiplayerCreature>>();

        /**
         *
         * Yaratık veri önbellek verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<ushort, MultiplayerCreatureItem> Creatures { get; set; } = new Dictionary<ushort, MultiplayerCreatureItem>();

        /**
         *
         * Kuyruğu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Queue<CreatureQueueItem> Queue { get; set; } = new Queue<CreatureQueueItem>();

        /**
         *
         * Meşgul Kuyruğu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Queue<CreatureQueueItem> BusyQueue { get; set; } = new Queue<CreatureQueueItem>();

        /**
         *
         * Aktif balıkları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<ushort, MultiplayerCreature> ActiveCreatureObjects { get; set; } = new Dictionary<ushort, MultiplayerCreature>();

        /**
         *
         * Aktif balıkların id'sini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private HashSet<ushort> ActiveCreatureIds { get; set; } = new HashSet<ushort>();

        /**
         *
         * Çerçeve Başına İşlem Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte ConsumptionPerFrame { get; set; } = 4;

        /**
         *
         * Mevcut çerçevede yapılan işlem sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte CurrentConsumptionCount { get; set; } = 0;

        /**
         *
         * Kuyruğun tüketilip/tüketilmediği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsRunning { get; set; } = false;

        /**
         *
         * Yaratığı hedefe yüzdürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SwimTo(ushort creatureId, Vector3 position, Quaternion rotation)
        {
            if (this.TryGetActiveCreatureObject(creatureId, out var creature))
            {
                creature.Movement.SwimTo(position, rotation);
            }
        }

        /**
         *
         * Yaratık kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RegisterCreature(ServerModel.WorldCreatureOwnershipItem creature)
        {
            this.Creatures[creature.Id] = new MultiplayerCreatureItem(creature.OwnerId, creature.Id, creature.Position.ToZeroVector3(), creature.Rotation.ToZeroQuaternion(), creature.TechType);
        }

        /**
         *
         * Yaratık kaydını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateCreature(ServerModel.WorldCreatureOwnershipItem creature)
        {
            if (this.TryGetCreature(creature.Id, out var data))
            {
                data.SetPositionAndRotation(creature.Position.ToZeroVector3(), creature.Rotation.ToZeroQuaternion());
                data.SetOwnership(creature.OwnerId);
            }
        }

        /**
         *
         * Aktif yaratık nesne kaydını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetActiveCreatureObject(ushort creatureId, out MultiplayerCreature creature)
        {
            return this.ActiveCreatureObjects.TryGetValue(creatureId, out creature);
        }

        /**
         *
         * Aktif yaratık nesne kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetActiveCreatureObject(ushort creatureId, MultiplayerCreature creature)
        {
            this.ActiveCreatureObjects[creatureId] = creature;
        }

        /**
         *
         * Aktif yaratık nesne kaydını siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveActiveCreatureObject(ushort creatureId)
        {
            this.ActiveCreatureObjects.Remove(creatureId);
        }

        /**
         *
         * Yaratık bana mı ait?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine(GameObject gameObject)
        {
            return IsMine(gameObject.GetIdentityId());
        }

        /**
         *
         * Yaratık bana mı ait?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine(string uniqueId)
        {
            if (!uniqueId.IsMultiplayerCreature())
            {
                return false;
            }

            return IsMine(uniqueId.ToCreatureId());
        }

        /**
         *
         * Yaratık bana mı ait?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine(ushort creatureId)
        {
            if (this.TryGetCreature(creatureId, out var creature))
            {
                return creature.IsMine();
            }

            return false;
        }

        /**
         *
         * Yaratık kaydını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetCreature(ushort creatureId, out MultiplayerCreatureItem creature)
        {
            return this.Creatures.TryGetValue(creatureId, out creature);
        }

        /**
         *
         * Yaratık kaydını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MultiplayerCreatureItem GetCreature(ushort creatureId)
        {
            this.TryGetCreature(creatureId, out var creature);
            return creature;
        }

        /**
         *
         * Aktif yaratıkları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerable<MultiplayerCreatureItem> GetActiveCreatures()
        {
            foreach (var creatureId in this.ActiveCreatureIds)
            {
                if (this.TryGetCreature(creatureId, out var creature))
                {
                    yield return creature;
                }
            }
        }

        /**
         *
         * Yaratık aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActiveCreature(ushort creatureId)
        {
            return this.ActiveCreatureIds.Contains(creatureId);
        }

        /**
         *
         * Yaratık aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void AddActiveCreature(ushort creatureId)
        {
            this.ActiveCreatureIds.Add(creatureId);
        }

        /**
         *
         * Yaratık aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void RemoveActiveCreature(ushort creatureId)
        {
            this.ActiveCreatureIds.Remove(creatureId);
        }

        /**
         *
         * Yaratık yok etme kuyruğuna ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ProcessToQueue(ushort creatureId, CreatureQueueAction action = null)
        {
            this.Queue.Enqueue(new CreatureQueueItem()
            {
                CreatureId = creatureId,
                IsProcess  = true,
                Action     = action,
            });

            this.ConsumeQueue();
            return true;
        }

        /**
         *
         * Yaratık doğma kuyruğuna ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SpawnToQueue(ushort creatureId)
        {
            if (this.IsActiveCreature(creatureId))
            {
                return false;
            }

            this.AddActiveCreature(creatureId);

            this.Queue.Enqueue(new CreatureQueueItem()
            {
                CreatureId = creatureId,
                IsSpawn    = true,
            });

            this.ConsumeQueue();
            return true;
        }

        /**
         *
         * Yaratık ölüm kuyruğuna ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool DeathToQueue(ushort creatureId)
        {
            if (!this.IsActiveCreature(creatureId))
            {
                return false;
            }

            this.RemoveActiveCreature(creatureId);

            this.Queue.Enqueue(new CreatureQueueItem()
            {
                CreatureId = creatureId,
                IsDeath    = true,
            });

            this.ConsumeQueue();
            return true;
        }

        /**
         *
         * Yaratık yok etme kuyruğuna ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveToQueue(ushort creatureId)
        {
            if (!this.IsActiveCreature(creatureId))
            {
                return false;
            }

            this.RemoveActiveCreature(creatureId);

            this.Queue.Enqueue(new CreatureQueueItem()
            {
                CreatureId = creatureId,
                IsSpawn = false,
            });

            this.ConsumeQueue();
            return true;
        }

        /**
         *
         * Yaratık yok etme kuyruğuna ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ChangeOwnershipToQueue(ushort creatureId)
        {
            this.Queue.Enqueue(new CreatureQueueItem()
            {
                CreatureId  = creatureId,
                IsChangeOWS = true,
            });

            this.ConsumeQueue();
            return true;
        }

        /**
         *
         * Mevcut çerçevede yapılan işlem sayısını sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ResetConsumption()
        {
            this.CurrentConsumptionCount = 0;
        }

        /**
         *
         * Mevcut çerçevede yapılan işlem sayısını arttırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void IncreaseConsumption()
        {
            this.CurrentConsumptionCount++;
        }

        /**
         *
         * Kuyruktaki işlemleri tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ConsumeQueue()
        {
            if (this.IsRunning)
            {
                return false;
            }

            CoroutineHost.StartCoroutine(this.ConsumeQueueAsync());
            return true;
        }

        /**
         *
         * Kuyruktaki nesneleri tüketir. (Async)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator ConsumeQueueAsync()
        {
            this.IsRunning = true;

            while (this.Queue.Count > 0)
            {
                var item = this.Queue.Dequeue();
                if (item.IsProcess == false)
                {
                    this.IncreaseConsumption();

                    if (this.CurrentConsumptionCount > this.ConsumptionPerFrame)
                    {
                        this.ResetConsumption();
                        yield return CoroutineUtils.waitForNextFrame;
                    }
                }

                if (this.TryGetCreature(item.CreatureId, out var creature))
                {
                    if (creature.IsBusy() && (item.IsProcess == false && item.IsChangeOWS == false))
                    {
                        Log.Info("BUSSY -> IsSpawn: " + item.IsSpawn + ", IsProcess: " + item.IsProcess + ", IsChangeOWS: " + item.IsChangeOWS + ", IsDeath: " + item.IsDeath);
                        this.BusyQueue.Enqueue(item);
                        continue;
                    }

                    if (item.IsSpawn)
                    {
                        var creatureObject = this.GetCreatureFromPool(creature.TechType);
                        if (creatureObject == null)
                        {
                            var result = new TaskResult<MultiplayerCreature>();

                            yield return this.SpawnCreature(creature.TechType, result);

                            creatureObject = result.Get();
                        }

                        try
                        {
                            creature.SetCreatureObject(creatureObject);
                            creature.Spawn();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"MultiplayerCreature.ConsumeQueueAsync - Spawn: {ex}");
                        }
                    }
                    else if (item.IsChangeOWS)
                    {
                        try
                        {
                            creature.ChangeOwnership();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"MultiplayerCreature.ConsumeQueueAsync - IsChangeOWS: {ex}");
                        }
                    }
                    else if (item.IsProcess)
                    {
                        var creatureObject = creature.GetCreatureObject();
                        if (creatureObject != null)
                        {
                            try
                            {
                                item.Action?.OnProcessCompleted?.Invoke(creatureObject, item);
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"MultiplayerCreature.ConsumeQueueAsync - IsProcess: {ex}");
                            }
                        }
                    }
                    else if (item.IsDeath)
                    {
                        var creatureObject = creature.GetCreatureObject();
                        if (creatureObject != null)
                        {
                            var result = new TaskResult<MultiplayerCreature>();

                            yield return this.SpawnCreature(creature.TechType, result, false);

                            creatureObject.OnKill(result.Get());
                        }

                        creature.Disable();
                    }                   
                    else
                    {
                        try
                        {
                            creature.Disable();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"MultiplayerCreature.ConsumeQueueAsync - Disable: {ex}");
                        }
                    }
                }
            }

            if (this.BusyQueue.Count > 0)
            {
                while (this.BusyQueue.Count > 0)
                {
                    this.Queue.Enqueue(this.BusyQueue.Dequeue());
                }

                yield return CoroutineUtils.waitForNextFrame;
            }

            this.IsRunning = false;

            if (this.Queue.Count > 0)
            {
                this.ConsumeQueue();
            }
        }

        /**
         *
         * Havuzdan yaratık alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private MultiplayerCreature GetCreatureFromPool(TechType techType)
        {
            if (this.CreaturePools.TryGetValue(techType, out var creatures))
            {
                return creatures.FirstOrDefault(q => !q.IsActive);
            }

            return null;
        }
        
        /**
         *
         * Havuz'a yaratık ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private MultiplayerCreature AddCreatureToPool(TechType techType, GameObject gameObject)
        {
            if (!this.CreaturePools.ContainsKey(techType))
            {
                this.CreaturePools.Add(techType, new List<MultiplayerCreature>());
            }

            if (this.CreaturePools.TryGetValue(techType, out var creatures))
            {
                var creature = new MultiplayerCreature(gameObject);
                creatures.Add(creature);

                return creature;
            }

            return null;
        }

        /**
         *
         * Havuz'da yaratık yoksa ekler varsa döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator SpawnCreature(TechType techType, TaskResult<MultiplayerCreature> item, bool addPool = true)
        {
            var creatureData = techType.GetCreatureData();
            if (creatureData != null)
            {
                GameObject gameObject = null;

                if (creatureData.SpawnLevel == CreatureSpawnLevel.Default)
                {
                    if (PrefabDatabase.TryGetPrefabFilename(techType.GetClassId(), out var filename))
                    {
                        var task = AddressablesUtility.InstantiateAsync(filename, null, default, default, false);

                        yield return task;

                        gameObject = task.GetResult();
                    }
                }
                else if (creatureData.SpawnLevel == CreatureSpawnLevel.Scene)
                {
                    var request = PrefabDatabase.GetPrefabAsync(techType.GetClassId());

                    yield return request;

                    if (request.TryGetPrefab(out var prefab))
                    {
                        gameObject = GameObject.Instantiate<GameObject>(prefab, null, default, default, false);
                    }
                }
                else if (creatureData.SpawnLevel == CreatureSpawnLevel.CustomAsync)
                {
                    var task = new TaskResult<GameObject>();

                    yield return creatureData.OnCustomCreatureSpawnAsync(task);

                    gameObject = task.Get();
                }
                else if (creatureData.SpawnLevel == CreatureSpawnLevel.Custom)
                {
                    gameObject = creatureData.OnCustomCreatureSpawn();
                }

                if (gameObject)
                {
                    if (gameObject.activeSelf)
                    {
                        gameObject.SetActive(false);
                    }

                    if (gameObject.TryGetComponent<LargeWorldEntity>(out var lwe))
                    {
                        lwe.enabled = false;
                    }

                    if (addPool)
                    {
                        item.Set(this.AddCreatureToPool(techType, gameObject));
                    }
                    else
                    {
                        item.Set(new MultiplayerCreature(gameObject));
                    }
                }
                else
                {
                    item.Set(null);
                }
            }
            else
            {
                item.Set(null);
            }
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.CreaturePools.Clear();
            this.Creatures.Clear();
            this.Queue.Clear();
            this.BusyQueue.Clear();
            this.ActiveCreatureObjects.Clear();
            this.ActiveCreatureIds.Clear();
            this.CurrentConsumptionCount = 0;
            this.IsRunning = false;
        }
    }
}

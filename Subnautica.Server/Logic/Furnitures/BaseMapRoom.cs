namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldStreamer;
    using Subnautica.Server.Abstracts;

    using UnityEngine;

    using MetadataModel    = Subnautica.Network.Models.Metadata;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class BaseMapRoom : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * ScannerCache nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, Dictionary<int, float>> ScannerCache { get; set; } = new Dictionary<string, Dictionary<int, float>>();

        /**
         *
         * Varsayılan Max Distance nesnesini barındırır. (300 metre)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float DefaultMaxDistance { get; set; } = 300f;

        /**
         *
         * MaxRequestDistance nesnesini barındırır. (ort 11 metre)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float MaxRequestDistance { get; set; } = 125f;

        /**
         *
         * Max Resource Limit nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int MaxResourceLimit { get; set; } = 20;

        /**
         *
         * IsSendAllPlayers nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsSendAllPlayers { get; set; } = false;

        /**
         *
         * FistRequests nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, HashSet<string>> FirstRequests { get; set; } = new Dictionary<string, HashSet<string>>();

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var construction in this.GetBaseMapRooms())
                {
                    this.SetRequestType(false);

                    var component = construction.Value.EnsureComponent<MetadataModel.BaseMapRoom>();
                    if (component == null)
                    {
                        continue;
                    }

                    var baseDeconstructable = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(construction.Value.UniqueId);
                    if (baseDeconstructable == null)
                    {
                        continue;
                    }

                    var mapRoom = baseDeconstructable.GetMapRoomFunctionality();
                    if (mapRoom == null)
                    {
                        continue;
                    }

                    if (!this.FirstRequests.ContainsKey(construction.Value.UniqueId))
                    {
                        this.FirstRequests.Add(construction.Value.UniqueId, new HashSet<string>());
                    }

                    if (this.ChargeVehicle(mapRoom, component.LeftDock.Vehicle))
                    {
                        Server.Core.Server.Instance.Logices.VehicleEnergyTransmission.VehicleEnergyUpdateQueue(component.LeftDock.Vehicle);
                    }

                    if (this.ChargeVehicle(mapRoom, component.RightDock.Vehicle))
                    {
                        Server.Core.Server.Instance.Logices.VehicleEnergyTransmission.VehicleEnergyUpdateQueue(component.RightDock.Vehicle);
                    }

                    this.ConsumeEnergy(mapRoom, component.IsScanning());

                    if (component.IsScanning())
                    {
                        this.InitializeMapRoom(construction.Value, component.StorageContainer, component.ScanTechType, component.IsChanged);

                        if (this.SyncResourceNodes(component))
                        {
                            this.SetRequestType(true);
                        }

                        var scanInterval = this.GetScanInterval(component.StorageContainer);
                        if (scanInterval + component.LastScanDate < Server.Core.Server.Instance.Logices.World.GetServerTime())
                        {
                            component.SetLastScanDate(Server.Core.Server.Instance.Logices.World.GetServerTime());

                            if (component.ResourceNodes.Count < this.MaxResourceLimit)
                            {

                                var itemId = this.FindItem(construction.Value, component.ScanTechType, component.ResourceNodes);
                                if (itemId.IsNotNull())
                                {
                                    component.AddResourceNode(itemId);

                                    this.SetRequestType(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (component.ResetNodes())
                        {
                            this.SetRequestType(true);
                        }
                    }

                    if (component.IsChanged)
                    {
                        component.IsChanged = false;

                        this.SetRequestType(true);
                    }

                    this.SendPacketToPlayers(construction.Value, component, this.IsSendAllPlayers);
                }
            }
        }

        /**
         *
         * Aracı şarj eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ChargeVehicle(global::MapRoomFunctionality mapRoom, WorldDynamicEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            var camera = entity.Component.GetComponent<WorldEntityModel.MapRoomCamera>();
            if (camera == null)
            {
                return false;
            }

            var energyAmount = Mathf.Min(camera.Battery.Capacity - camera.Battery.Charge, 2.5f);

            if (Core.Server.Instance.Logices.PowerConsumer.HasPower(mapRoom.powerConsumer, energyAmount))
            {
                if (camera.Battery.AddEnergy(energyAmount, out var addedEnergyAmount))
                {
                    Core.Server.Instance.Logices.PowerConsumer.ConsumePower(mapRoom.powerConsumer, addedEnergyAmount, out var _);
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * İstek türünü değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetRequestType(bool isAll)
        {
            this.IsSendAllPlayers = isAll;
        }

        /**
         *
         * Harita odasını önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool InitializeMapRoom(ConstructionItem construction, MetadataModel.StorageContainer storageContainer, TechType techType, bool isChanged = false)
        {
            if (isChanged)
            {
                this.ScannerCache.Remove(construction.UniqueId);
            }

            if (this.ScannerCache.ContainsKey(construction.UniqueId))
            {
                return false;
            }

            this.ScannerCache[construction.UniqueId] = new Dictionary<int, float>();

            var maxDistance = this.GetMaxDistance(storageContainer);

            foreach (var item in this.GetSpawnPoints(techType))
            {
                var spawnPoint = Network.WorldStreamer.GetSlotById(item.Value.SlotId);
                if (spawnPoint == null)
                {
                    continue;
                }

                var distance = spawnPoint.LeashPosition.Distance(construction.PlacePosition);
                if (distance > maxDistance)
                {
                    continue;
                }

                this.ScannerCache[construction.UniqueId].Add(spawnPoint.SlotId, distance);
            }

            this.ScannerCache[construction.UniqueId] = this.ScannerCache[construction.UniqueId].OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return true;
        }

        /**
         *
         * Oda tarama genişliğini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateScanRange(ConstructionItem construction, MetadataModel.BaseMapRoom baseMapRoom)
        {
            this.InitializeMapRoom(construction, baseMapRoom.StorageContainer, baseMapRoom.ScanTechType, true);
        }

        /**
         *
         * SpawnPoints değerlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerable<KeyValuePair<int, ZeroSpawnPointSimple>> GetSpawnPoints(TechType techType)
        {
            if (techType.IsFragment())
            {
                return Server.Core.Server.Instance.Logices.WorldStreamer.GetSpawnPoints().Where(q => q.Value.TechType.IsFragment());
            }

            if (techType.IsCreatureEgg())
            {
                return Server.Core.Server.Instance.Logices.WorldStreamer.GetSpawnPoints().Where(q => q.Value.TechType.IsCreatureEgg());
            }

            return Server.Core.Server.Instance.Logices.WorldStreamer.GetSpawnPoints().Where(q => q.Value.TechType == techType);
        }

        /**
         *
         * Oyunculara paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendPacketToPlayers(ConstructionItem construction, MetadataModel.BaseMapRoom mapRoom, bool isAll)
        {
            if (this.IsPacketSendable(construction, isAll))
            {
                var packet = this.GetRequestPacket(construction.UniqueId, mapRoom.ResourceNodes);

                foreach (var profile in Server.Core.Server.Instance.GetPlayers())
                {
                    if (isAll || construction.PlacePosition.Distance(profile.Position) < this.MaxRequestDistance)
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                    else if (profile.IsFullConnected && this.FirstRequests.TryGetValue(construction.UniqueId, out var players) && !players.Contains(profile.UniqueId))
                    {
                        players.Add(profile.UniqueId);

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
        }

        /**
         *
         * İstek paketini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private BaseMapRoomTransmissionArgs GetRequestPacket(string uniqueId, HashSet<string> resourceNodes)
        {
            BaseMapRoomTransmissionArgs packet = new BaseMapRoomTransmissionArgs()
            {
                UniqueId = uniqueId,
                Items    = new List<BaseMapRoomTransmissionItem>()
            };

            foreach (var itemId in resourceNodes)
            {
                if (itemId.IsWorldStreamer())
                {
                    var spawnPoint = Network.WorldStreamer.GetSlotById(itemId.WorldStreamerToSlotId());
                    if (spawnPoint != null)
                    {
                        if (spawnPoint.TechType.IsCreature())
                        {
                            if (Server.Core.Server.Instance.Logices.CreatureWatcher.TryGetCreature(itemId, out var creature))
                            {
                                packet.Items.Add(new BaseMapRoomTransmissionItem(itemId, creature.Position.Compress()));
                            }
                            else
                            {
                                packet.Items.Add(new BaseMapRoomTransmissionItem(itemId, spawnPoint.LeashPosition.Compress()));
                            }
                        }
                        else
                        {
                            packet.Items.Add(new BaseMapRoomTransmissionItem(itemId, spawnPoint.LeashPosition.Compress()));
                        }
                    }
                }
                else
                {
                    var entity = Server.Core.Server.Instance.Storages.World.GetDynamicEntity(itemId);
                    if (entity != null)
                    {
                        packet.Items.Add(new BaseMapRoomTransmissionItem(itemId, entity.Position.Compress()));
                    }
                }
            }

            return packet;
        }

        /**
         *
         * Paket gönderim durumu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPacketSendable(ConstructionItem construction, bool isAll)
        {
            if (isAll)
            {
                return true;
            }

            foreach (var profile in Server.Core.Server.Instance.GetPlayers())
            {
                if (construction.PlacePosition.Distance(profile.Position) < this.MaxRequestDistance || (profile.IsFullConnected && this.FirstRequests.TryGetValue(construction.UniqueId, out var players) && !players.Contains(profile.UniqueId)))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Kaynakları senkronize eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool SyncResourceNodes(MetadataModel.BaseMapRoom mapRoom)
        {
            if (mapRoom.ResourceNodes.Count <= 0)
            {
                return false;
            }

            var serverTime = Server.Core.Server.Instance.Logices.World.GetServerTime();
            var isRemoved  = false; 

            foreach (var uniqueId in mapRoom.ResourceNodes.ToList())
            {
                if (uniqueId.IsWorldStreamer())
                {
                    var spawnPoint = Network.WorldStreamer.GetSlotById(uniqueId.WorldStreamerToSlotId());
                    if (spawnPoint == null || !spawnPoint.IsRespawnable(serverTime))
                    {
                        isRemoved = true;

                        mapRoom.ResourceNodes.Remove(uniqueId);
                    }
                }
                else
                {
                    var entity = Server.Core.Server.Instance.Storages.World.GetDynamicEntity(uniqueId);
                    if (entity == null)
                    {
                        isRemoved = true;

                        mapRoom.ResourceNodes.Remove(uniqueId);
                    }
                }
            }

            return isRemoved;
        }

        /**
         *
         * Nesne arar ve id döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string FindItem(ConstructionItem construction, TechType techType, HashSet<string> ignoreIds)
        {
            if (this.ScannerCache.TryGetValue(construction.UniqueId, out var items))
            {
                var serverTime = Server.Core.Server.Instance.Logices.World.GetServerTime();

                foreach (var item in items)
                {
                    var spawnPoint = Network.WorldStreamer.GetSlotById(item.Key);
                    if (spawnPoint.IsRespawnable(serverTime))
                    {
                        var itemId = spawnPoint.SlotId.ToWorldStreamerId();
                        if (ignoreIds.Contains(itemId))
                        {
                            continue;
                        }

                        return itemId;
                    }
                }
            }

            foreach (var item in Server.Core.Server.Instance.Storages.World.Storage.DynamicEntities)
            {
                if (item.TechType == techType && !ignoreIds.Contains(item.UniqueId) && item.Position.Distance(construction.PlacePosition) < this.DefaultMaxDistance)
                {
                    return item.UniqueId;
                }
            }

            return null;
        }

        /**
         *
         * Oyuncuya çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerDisconnected(string uniqueId)
        {
            foreach (var item in this.FirstRequests.Values)
            {
                item.Remove(uniqueId);
            }
        }

        /**
         *
         * Enerji tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ConsumeEnergy(MapRoomFunctionality mapRoom, bool isScanning)
        {
            var energyAmount = isScanning ? 0.5f : 0.15f;

            if (Core.Server.Instance.Logices.PowerConsumer.HasPower(mapRoom.powerConsumer, energyAmount))
            {
                return Core.Server.Instance.Logices.PowerConsumer.ConsumePower(mapRoom.powerConsumer, energyAmount, out var _);
            }

            return false;
        }

        /**
         *
         * Tarama süresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetScanInterval(MetadataModel.StorageContainer storageContainer)
        {
            if (storageContainer == null)
            {
                return 14f;
            }

            return Mathf.Max(1f, (float)(14.0 - (double)storageContainer.GetCount(TechType.MapRoomUpgradeScanSpeed) * 3.0));
        }

        /**
         *
         * Max uzaklığı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetMaxDistance(MetadataModel.StorageContainer construction)
        {
            if (construction == null)
            {
                return this.DefaultMaxDistance * this.DefaultMaxDistance;
            }

            var maxDistance = this.DefaultMaxDistance + (construction.GetCount(TechType.MapRoomUpgradeScanRange) * 50f);
            return maxDistance * maxDistance;
        }

        /**
         *
         * Hoverpad'leri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<KeyValuePair<string, ConstructionItem>> GetBaseMapRooms()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.ConstructedAmount == 1f && q.Value.TechType == TechType.BaseMapRoom).ToList();
        }
    }
}

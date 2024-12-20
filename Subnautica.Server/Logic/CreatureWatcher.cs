namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Core;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Creatures;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Events.EventArgs;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CreatureWatcher : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(2000f);

        /**
         *
         * PlayerActiveCreatures nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, HashSet<ushort>> PlayerActiveCreatures { get; set; } = new Dictionary<byte, HashSet<ushort>>();

        /**
         *
         * PlayerLoadedCreatures nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, HashSet<ushort>> PlayerLoadedCreatures { get; set; } = new Dictionary<byte, HashSet<ushort>>();

        /**
         *
         * PlayerOwnershipRequests nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, List<WorldCreatureOwnershipItem>> PlayerOwnershipRequests { get; set; } = new Dictionary<byte, List<WorldCreatureOwnershipItem>>();

        /**
         *
         * Konum isteklerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, WorldCreaturePositionArgs> PlayerPositionRequests { get; set; } = new Dictionary<byte, WorldCreaturePositionArgs>();

        /**
         *
         * Creatures barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<ushort, MultiplayerCreatureItem> Creatures { get; set; } = null;

        /**
         *
         * CreaturesByWorldStreamerIds barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, ushort> CreaturesByWorldStreamerIds { get; set; } = new Dictionary<string, ushort>();

        /**
         *
         * ActiveCreatures barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */   
        private HashSet<MultiplayerCreatureItem> ActiveCreatures { get; set; } = new HashSet<MultiplayerCreatureItem>();

        /**
         *
         * Yeniden doğma kuyruğu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<ushort, StopwatchItem> RespawnQueue { get; set; } = new Dictionary<ushort, StopwatchItem>();

        /**
         *
         * Merkeze dönme kuyruğu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<ushort, StopwatchItem> StayAtLeashQueue { get; set; } = new Dictionary<ushort, StopwatchItem>();

        /**
         *
         * LastCreatureId barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ushort LastCreatureId { get; set; } = 1;

        /**
         *
         * IsLoading barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsLoading { get; set; } = false;

        /**
         *
         * IsLoading barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsForceTrigger { get; set; } = false;

        /**
         *
         * Anında tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ImmediatelyTrigger()
        {
            this.IsForceTrigger = true;
            this.OnUpdate(0f);
            return true;
        }

        /**
         *
         * Olayı tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void TriggerAction(NetworkPacket actionPacket)
        {
            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected)
                {
                    player.SendPacket(actionPacket);
                }
            }
        }

        /**
         *
         * Olayı temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ClearAction(MultiplayerCreatureItem creature, float delay = 0f)
        {
            if (delay <= 0f)
            {
                creature.ClearAction(true);
            }
            else
            {
                Server.Instance.Logices.Timing.AddQueue(creature.Id.ToCreatureStringId(), () => { creature?.ClearAction(true); }, delay);
            }
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (API.Features.World.IsLoaded && this.LoadCreatures() && (this.IsForceTrigger || this.Timing.IsFinished()))
            {
                this.IsForceTrigger = false;
                this.Timing.Restart();

                this.InitializePlayers();
                this.RespawnDeadCreatures();
                this.StayAtLeashCreatures();
                this.UpdateClosestCreatures();
                this.UpdateCreatureOwnerships();
                this.SendOwnershipPacketToAllClient();
            }
        }

        /**
         *
         * Her şey hazır mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLoaded()
        {
            return this.IsLoading == false && this.Creatures != null;
        }

        /**
         *
         * Yaratıkları önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool LoadCreatures()
        {
            if (this.IsLoading)
            {
                return false;
            }

            if (this.Creatures != null)
            {
                return true;
            }

            if (!Server.Instance.Logices.WorldStreamer.IsGeneratedWorld())
            {
                return false;
            }
            
            this.IsLoading = true;
            this.Creatures = new Dictionary<ushort, MultiplayerCreatureItem>();
            this.CreaturesByWorldStreamerIds = new Dictionary<string, ushort>();

            foreach (var spawnPoint in Server.Instance.Logices.WorldStreamer.GetSpawnPoints())
            {
                if (spawnPoint.Value.TechType.IsCreature(true) || spawnPoint.Value.TechType == TechType.CrashHome)
                {
                    var originalSpawnPoint = Network.WorldStreamer.GetSlotById(spawnPoint.Value.SlotId);
                    if (originalSpawnPoint != null && originalSpawnPoint.LeashPosition != null)
                    {
                        if (spawnPoint.Value.TechType == TechType.CrashHome)
                        {
                            if (spawnPoint.Value.IsRespawnable(Server.Instance.Logices.World.GetServerTime()))
                            {
                                this.RegisterCreature(TechType.Crash, originalSpawnPoint.LeashPosition, new ZeroQuaternion(), spawnPoint.Value.SlotId.ToWorldStreamerId());
                            }
                        }
                        else
                        {
                            this.RegisterCreature(originalSpawnPoint.TechType, originalSpawnPoint.LeashPosition, originalSpawnPoint.LeashRotation, spawnPoint.Value.SlotId.ToWorldStreamerId());
                        }
                    }
                }
            }

            this.IsLoading = false;
            return true;
        }

        /**
         *
         * Yaratık kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ushort RegisterCreature(TechType techType, ZeroVector3 leashPosition, ZeroQuaternion leashRotation, string worldStreamerId = null)
        {
            if (this.Creatures.Count >= ushort.MaxValue)
            {
                return 0;
            }

            if (!techType.IsSynchronizedCreature())
            {
                return 0;
            }

            while (this.Creatures.ContainsKey(this.LastCreatureId))
            {
                this.LastCreatureId++;

                if (this.LastCreatureId >= ushort.MaxValue)
                {
                    this.LastCreatureId = 1;
                }
            }

            var creature = new MultiplayerCreatureItem(0, this.LastCreatureId, leashPosition, leashRotation, techType);
            creature.SetPositionAndRotation(leashPosition, leashRotation);

            if (worldStreamerId.IsNotNull())
            {
                creature.SetWorldStreamerId(worldStreamerId);

                this.CreaturesByWorldStreamerIds.Add(worldStreamerId, creature.Id);
            }

            this.Creatures.Add(creature.Id, creature);
            return creature.Id;
        }

        /**
         *
         * Yaratık kaydını siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UnRegisterCreature(ushort creatureId)
        {
            if (this.TryGetCreature(creatureId, out var creature))
            {
                foreach (var player in Server.Instance.GetPlayers())
                {
                    if (this.PlayerActiveCreatures.TryGetValue(player.PlayerId, out var activeCreatures) && activeCreatures.Contains(creatureId))
                    {
                        this.ChangeCreatureOwnership(player.PlayerId, creatureId, 0, false);
                    }

                    if (this.PlayerLoadedCreatures.TryGetValue(player.PlayerId, out var loadedCreatures) && loadedCreatures.Contains(creatureId))
                    {
                        loadedCreatures.Remove(creatureId);
                    }
                }

                if (this.IsActiveCreature(creatureId))
                {
                    this.DisableCreature(creature);
                    this.SendOwnershipPacketToAllClient();
                }

                this.StayAtLeashQueue.Remove(creatureId);
                this.RespawnQueue.Remove(creatureId);
                this.Creatures.Remove(creatureId);

                if (creature.WorldStreamerId.IsNotNull())
                {
                    this.CreaturesByWorldStreamerIds.Remove(creature.WorldStreamerId);
                }

                return true;
            }

            return false;
        }

        /**
         *
         * Yakınlardaki balıkları önbelleğe yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateClosestCreatures()
        {
            foreach (var creature in this.Creatures)
            {
                if (creature.Value.IsExistsOwnership())
                {
                    continue;
                }

                if (this.IsCreatureActivable(creature.Value))
                {
                    this.ActivateCreature(creature.Value);
                }
            }
        }

        /**
         *
         * Ölü balıkları yeniden canlandırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RespawnDeadCreatures()
        {
            if (this.RespawnQueue.Count > 0)
            {
                foreach (var item in this.RespawnQueue.ToList())
                {
                    if (item.Value.IsFinished())
                    {
                        if (this.TryGetCreature(item.Key, out var creature))
                        {
                            creature.LiveMixin.ResetHealth();
                        }

                        this.RespawnQueue.Remove(item.Key);
                    }
                }
            }
        }

        /**
         *
         * Balıkların konumlarını başlangıça döndürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StayAtLeashCreatures()
        {
            if (this.StayAtLeashQueue.Count > 0)
            {
                foreach (var item in this.StayAtLeashQueue.ToList())
                {
                    if (item.Value.IsFinished())
                    {
                        if (this.TryGetCreature(item.Key, out var creature))
                        {
                            this.ResetCreaturePosition(creature);
                        }

                        this.StayAtLeashQueue.Remove(item.Key);
                    }
                }
            }
        }

        /**
         *
         * Başlatılmamış oyuncu yapılandırmalarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void InitializePlayers()
        {
            foreach (var player in Server.Instance.GetPlayers())
            {
                if (!this.PlayerActiveCreatures.ContainsKey(player.PlayerId))
                {
                    this.PlayerActiveCreatures.Add(player.PlayerId, new HashSet<ushort>());
                }

                if (!this.PlayerLoadedCreatures.ContainsKey(player.PlayerId))
                {
                    this.PlayerLoadedCreatures.Add(player.PlayerId, new HashSet<ushort>());
                }

                if (!this.PlayerOwnershipRequests.ContainsKey(player.PlayerId))
                {
                    this.PlayerOwnershipRequests.Add(player.PlayerId, new List<WorldCreatureOwnershipItem>());
                }

                if (!this.PlayerPositionRequests.ContainsKey(player.PlayerId))
                {
                    this.PlayerPositionRequests.Add(player.PlayerId, new WorldCreaturePositionArgs());
                }
            }
        }

        /**
         *
         * Yaratığı aktifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsActiveCreature(ushort creatureId)
        {
            return this.ActiveCreatures.Any(q => q.Id == creatureId);
        }

        /**
         *
         * Yaratığı aktifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ActivateCreature(MultiplayerCreatureItem creature)
        {
            if (!this.IsActiveCreature(creature.Id))
            {
                this.ActiveCreatures.Add(creature);

                this.StayAtLeashQueue.Remove(creature.Id);
            }
        }

        /**
         *
         * Yaratığı pasifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void DisableCreature(MultiplayerCreatureItem creature)
        {
            this.ActiveCreatures.Remove(creature);

            if (!creature.LiveMixin.IsDead && creature.Data.StayAtLeashPositionWhenPassive > -1f && creature.Position.Distance(creature.LeashPosition) > creature.Data.StayAtLeashPositionWhenPassive * creature.Data.StayAtLeashPositionWhenPassive)
            {
                this.StayAtLeashQueue[creature.Id] = new StopwatchItem(creature.Data.StayAtLeashPositionTime);
            }
        }

        /**
         *
         * Yaratık konumlarını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateCreaturePosition(ushort creatureId, ZeroVector3 position, ZeroQuaternion rotation, bool updateCell = false)
        {
            if (this.TryGetCreature(creatureId, out var creature))
            {
                creature.SetPositionAndRotation(position, rotation);
            }
        }

        /**
         *
         * Yaratıkların hepsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerable<KeyValuePair<ushort, MultiplayerCreatureItem>> GetCreatures(TechType techType = TechType.None)
        {
            if (techType == TechType.None)
            {
                return this.Creatures.ToList();    
            }

            return this.Creatures.Where(q => q.Value.TechType == techType);
        }

        /**
         *
         * Yaratığı döner.
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
         * Yaratığı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetCreature(string worldStreamerId, out MultiplayerCreatureItem creature)
        {
            if (this.CreaturesByWorldStreamerIds.TryGetValue(worldStreamerId, out var creatureId))
            {
                return this.TryGetCreature(creatureId, out creature);
            }

            creature = null;
            return false;
        }

        /**
         *
         * Yaratığı aktif edilebilirlik durumunu döner. döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCreatureActivable(MultiplayerCreatureItem creature)
        {
            if (creature.IsBusy())
            {
                return true;
            }

            if (creature.LiveMixin.IsDead)
            {
                return false;
            }

            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected && player.CanSeeTheCreature(creature))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Yaratık sahipliğini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ChangeCreatureOwnership(byte playerId, ushort creatureId, byte newOwnerId, bool isCreatureDead = false)
        {
            if (this.TryGetCreature(creatureId, out var creature))
            {
                if (this.PlayerOwnershipRequests.TryGetValue(playerId, out var requests))
                {
                    if (this.PlayerLoadedCreatures.TryGetValue(playerId, out var loadedCreatures) && loadedCreatures.Contains(creatureId))
                    {
                        if (newOwnerId == 0)
                        {
                            requests.Add(new WorldCreatureOwnershipItem(newOwnerId, creature.Id, isCreatureDead ? -1 : 0, 0, TechType.None));
                        }
                        else
                        {
                            requests.Add(new WorldCreatureOwnershipItem(newOwnerId, creature.Id, creature.Position.Compress(), creature.Rotation.Compress(), TechType.None));
                        }
                    }
                    else
                    {
                        requests.Add(new WorldCreatureOwnershipItem(newOwnerId, creature.Id, creature.LeashPosition.Compress(), creature.LeashRotation.Compress(), creature.TechType));
                    }
                }
            }

            if (newOwnerId > 0)
            {
                if (this.PlayerActiveCreatures.TryGetValue(playerId, out var values) && !values.Contains(creatureId))
                {
                    values.Add(creatureId);
                }

                if (this.PlayerLoadedCreatures.TryGetValue(playerId, out var loadedCreatures) && !loadedCreatures.Contains(creatureId))
                {
                    loadedCreatures.Add(creatureId);
                }
            }
            else
            {
                if (this.PlayerActiveCreatures.TryGetValue(playerId, out var values))
                {
                    values.Remove(creatureId);
                }
            }

            return true;
        }

        /**
         *
         * Oyunculara yaratık sahip değişim paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendOwnershipPacketToAllClient()
        {
            foreach (var item in this.PlayerOwnershipRequests)
            {
                if (item.Value.Count > 0)
                {
                    var profile = Server.Instance.GetPlayer(item.Key);
                    if (profile == null)
                    {
                        item.Value.Clear();
                        continue;
                    }

                    foreach (var creatures in item.Value.Split<WorldCreatureOwnershipItem>(35))
                    {
                        ServerModel.WorldCreatureOwnershipChangedArgs request = new ServerModel.WorldCreatureOwnershipChangedArgs()
                        {
                            Creatures = creatures.ToList(),
                        };

                        profile.SendPacket(request);
                    }

                    item.Value.Clear();
                }
            }
        }

        /**
         *
         * Yakınlarda aktif olan balıkları oyunculara atar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateCreatureOwnerships()
        {
            foreach (var creature in this.ActiveCreatures.ToList())
            {
                var oldOwnershipId = creature.OwnerId;

                if (creature.LiveMixin.IsDead)
                {
                    creature.SetOwnership(0);
                    creature.ClearBusyOwnerId();

                    this.ResetCreaturePosition(creature);

                    if (creature.Data.IsRespawnable)
                    {
                        this.RespawnQueue[creature.Id] = new StopwatchItem(creature.Data.GetRespawnDuration() * 1000f);
                    }
                }
                else if (creature.IsBusy())
                {
                    creature.SetOwnership(creature.BusyOwnerId);
                }
                else
                {
                    if (creature.IsExistsOwnership())
                    {
                        var owner = Server.Instance.GetPlayer(creature.OwnerId);
                        if (owner == null || !owner.CanSeeTheCreature(creature, true))
                        {
                            creature.SetOwnership(this.FindCreatureOwnership(creature));
                        }
                    }
                    else
                    {
                        creature.SetOwnership(this.FindCreatureOwnership(creature));
                    }
                }

                foreach (var player in Server.Instance.GetPlayers())
                {
                    if (player.IsFullConnected && this.PlayerActiveCreatures.TryGetValue(player.PlayerId, out var creatures))
                    {
                        if (creature.OwnerId == 0)
                        {
                            if (creatures.Contains(creature.Id))
                            {
                                this.ChangeCreatureOwnership(player.PlayerId, creature.Id, 0, creature.LiveMixin.IsDead);
                            }
                        }
                        else if (oldOwnershipId != creature.OwnerId)
                        {
                            if (creature.IsBusy() || player.CanSeeTheCreature(creature, creature.IsMine(player.PlayerId)))
                            {
                                this.ChangeCreatureOwnership(player.PlayerId, creature.Id, creature.OwnerId);
                            }
                            else
                            {
                                this.ChangeCreatureOwnership(player.PlayerId, creature.Id, 0);
                            }
                        }
                        else
                        {
                            if (creature.IsBusy() || player.CanSeeTheCreature(creature, creature.IsMine(player.PlayerId)))
                            {
                                if (!creatures.Contains(creature.Id))
                                {
                                    this.ChangeCreatureOwnership(player.PlayerId, creature.Id, creature.OwnerId);
                                }
                            }
                            else
                            {
                                if (creatures.Contains(creature.Id))
                                {
                                    this.ChangeCreatureOwnership(player.PlayerId, creature.Id, 0);
                                }
                            }
                        }
                    }
                }

                if (!creature.IsExistsOwnership())
                {
                    this.DisableCreature(creature);
                }
            }
        }

        /**
         *
         * Yaratık konumunu sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ResetCreaturePosition(MultiplayerCreatureItem creature)
        {
            this.UpdateCreaturePosition(creature.Id, creature.LeashPosition, creature.LeashRotation, true);
        }

        /**
         *
         * Yaratık konum verisi alınınca tetiklenir ve işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreaturePositionDataReceived(byte requesterId, List<WorldCreaturePosition> positions)
        {
            foreach (var item in positions)
            {
                this.UpdateCreaturePosition(item.CreatureId, item.Position.ToZeroVector3(), item.Rotation.ToZeroQuaternion());
            }

            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.PlayerId == requesterId)
                {
                    continue;
                }

                if (player.IsFullConnected && this.PlayerActiveCreatures.TryGetValue(player.PlayerId, out var activeCreatures) && this.PlayerPositionRequests.TryGetValue(player.PlayerId, out var requests))
                {
                    foreach (var item in positions)
                    {
                        if (activeCreatures.Contains(item.CreatureId))
                        {
                            requests.Positions.Add(item);
                        }
                    }

                    if (requests.Positions.Count > 0)
                    {
                        player.SendPacket(requests);
    
                        requests.Positions.Clear();
                    }
                }
            }
        }

        /**
         *
         * Yaratık animasyon verisi alınınca tetiklenir ve işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnAnimationDataReceived(ushort requesterId, HashSet<CreatureAnimationItem> animations)
        {
            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.PlayerId == requesterId)
                {
                    continue;
                }

                if (player.IsFullConnected && this.PlayerActiveCreatures.TryGetValue(player.PlayerId, out var activeCreatures))
                {
                    CreatureAnimationArgs request = new CreatureAnimationArgs();

                    foreach (var item in animations)
                    {
                        if (activeCreatures.Contains(item.CreatureId))
                        {
                            request.Animations.Add(item);
                        }
                    }

                    if (request.Animations.Count > 0)
                    {
                        player.SendPacket(request);
                    }
                }
            }
        }

        /**
         *
         * Oyuncu tamamen bağlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerFullConnected(PlayerFullConnectedEventArgs ev)
        {
            this.ImmediatelyTrigger();

            foreach (var creature in this.ActiveCreatures)
            {
                if (creature.IsActionExists())
                {
                    ev.Player.SendPacket(creature.GetAction());
                }
            }
        }

        /**
         *
         * Oyuncu ayrıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerDisconnected(byte playerId)
        {
            foreach (var item in this.PlayerActiveCreatures)
            {
                foreach (var creatureId in item.Value)
                {
                    if (this.TryGetCreature(creatureId, out var creature))
                    {
                        if (creature.BusyOwnerId == playerId)
                        {
                            this.OnCreatureDead(creature);
                        }
                    }
                }
            }

            this.PlayerActiveCreatures.Remove(playerId);
            this.PlayerLoadedCreatures.Remove(playerId);
            this.PlayerOwnershipRequests.Remove(playerId);
            this.PlayerPositionRequests.Remove(playerId);
            this.ImmediatelyTrigger();
        }

        /**
         *
         * Yaratık öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureDead(MultiplayerCreatureItem creature)
        {
            creature.ClearAction(true);

            Server.Instance.Logices.Timing.RemoveFromQueue(creature.Id.ToCreatureStringId());
        }

        /**
         *
         * Yaratık öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCallSoundTriggered(MultiplayerCreatureItem creature, ServerModel.CreatureCallArgs packet)
        {
            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected && this.PlayerActiveCreatures.TryGetValue(player.PlayerId, out var creatures) && creatures.Contains(creature.Id))
                {
                    player.SendPacket(packet);
                }
            }
        }

        





























        /**
         *
         * Nesne için yeni sahip arar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte FindCreatureOwnership(MultiplayerCreatureItem creature)
        {
            if (creature.IsBusy())
            {
                return creature.BusyOwnerId;
            }

            byte ownershipId = 0;
            
            var visibility   = creature.Data.GetVisibilityDistance();
            var lastDistance = 100000f;
            var hostRange    = visibility * 0.75f;

            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.IsFullConnected)
                {
                    // CanSeeTheCreature
                    var distance = player.Position.Distance(creature.Position);
                    if (distance < visibility)
                    {
                        if (player.IsHost && distance < hostRange)
                        {
                            return player.PlayerId;
                        }
                        else if (distance < lastDistance)
                        {
                            ownershipId  = player.PlayerId;
                            lastDistance = distance;
                        }
                    }
                }
            }

            return ownershipId;
        }        
    }
}

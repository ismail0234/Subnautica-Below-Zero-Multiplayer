namespace Subnautica.Server.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteNetLib;

    using MessagePack;

    using Story;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Creatures;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.Player;
    using Subnautica.Network.Models.Storage.Story.StoryGoals;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Events;
    using Subnautica.Server.Events.EventArgs;

    using ServerModel = Subnautica.Network.Models.Server;

    [MessagePackObject]
    public class AuthorizationProfile
    {
        /**
         *
         * Oyuncu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        private string ipPortAddress;

        /**
         *
         * Oyuncu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public byte PlayerId { get; set; }

        /**
         *
         * Benzersiz Oyuncu Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string UniqueId { get; set; }

        /**
         *
         * Kullanıcı ip adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string IpAddress { get; set; }

        /**
         *
         * Kullanıcı IP & Port Adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string IpPortAddress 
        { 
            get
            {
                return this.ipPortAddress;
            }
            set
            {
                this.ipPortAddress = value;

                if (this.ipPortAddress.IsNotNull())
                {
                    this.IpAddress = this.ipPortAddress.Split(':')[0].Trim();
                }
                else
                {
                    this.IpAddress = null;
                }
            }
        }

        /**
         *
         * Oyuncu barındırıcı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsHost { get; set; } = false;

        /**
         *
         * Doğrulama Başarılı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsAuthorized { get; set; } = false;

        /**
         *
         * Tamamen Giriş Yapıldı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsFullConnected { get; set; } = false;

        /**
         *
         * NetPeer Bağlantısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public NetPeer NetPeer { get; set; }

        /**
         *
         * Oyuncunun kullandığı mevcut araç.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string VehicleId { get; set; }

        /**
         *
         * Mevcut hava durumu profili
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string WeatherProfileId { get; set; }

        /**
         *
         * Kullanılan araçları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsStoryCinematicModeActive { get; set; } = false;

        /**
         *
         * Kullanılan odayı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public string UsingRoomId { get; set; }

        /**
         *
         * Son Saldırıya uğrama zamanı.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public float LastAttackTime { get; set; }

        /**
         *
         * IsInVoidBiome Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [IgnoreMember]
        public bool IsInVoidBiome { get; private set; }

        /**
         *
         * Kullanıcı Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string PlayerName { get; set; }

        /**
         *
         * SubrootId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string SubrootId { get; set; }

        /**
         *
         * InteriorId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string InteriorId { get; set; }

        /**
         *
         * Mevcut Sağlık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float Health { get; set; } = 100f;

        /**
         *
         * Su Miktarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float Water { get; set; } = 90.5f;

        /**
         *
         * Açlık Miktarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public float Food { get; set; } = 50.5f;

        /**
         *
         * Oyuncu Konumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroVector3 Position { get; set; } = new ZeroVector3();  

        /**
         *
         * Oyuncu Açısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public ZeroQuaternion Rotation { get; set; } = new ZeroQuaternion();

        /**
         *
         * Envanter eşyaları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public StorageContainer InventoryItems { get; set; }

        /**
         *
         * Ekipman eşyaları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public byte[] Equipments { get; set; }

        /**
         *
         * Ekipman eşyala Id'leri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public Dictionary<string, string> EquipmentSlots { get; set; } = new Dictionary<string, string>(); 

        /**
         *
         * Hızlı slot id'leri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public string[] QuickSlots { get; set; }

        /**
         *
         * Aktif slot değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public int ActiveSlot { get; set; }

        /**
         *
         * Teknoloji pinlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public List<TechType> ItemPins { get; set; } = new List<TechType>();    

        /**
         *
         * PDA Bildirimlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public HashSet<NotificationItem> PdaNotifications { get; set; } = new HashSet<NotificationItem>();

        /**
         *
         * Kullanılan araçları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(15)]
        public HashSet<TechType> UsedTools { get; set; } = new HashSet<TechType>();

        /**
         *
         * Kişiye özel hedefleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(16)]
        public HashSet<ZeroStoryGoal> SpecialGoals { get; set; } = new HashSet<ZeroStoryGoal>();

        /**
         *
         * RespawnPointId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(17)]
        public string RespawnPointId { get; set; }

        /**
         *
         * IsInitialEquipmentAdded değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(18)]
        public bool IsInitialEquipmentAdded { get; set; }

        /**
         *
         * LastHypnotizeTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(19)]
        public float LastHypnotizeTime { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile()
        {
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile(NetPeer netPeer)
        {
            this.IpPortAddress = netPeer.ToString();
            this.NetPeer       = netPeer;
            this.PlayerId      = Server.Instance.GetNextPlayerId();
        }

        /**
         *
         * Doğrulama ve başlatma işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile Initialize(string playerName, string uniqueId)
        {
            uniqueId = Tools.CreateMD5(uniqueId);
            if (uniqueId.IsNull())
            {
                return null;
            }

            var profile = Server.Instance.Storages.Player.GetPlayerData(uniqueId, playerName);
            if (profile == null)
            {
                Log.Error("PLAYER DATA ERROR");
                return null;
            }
            
            profile.IsAuthorized  = true;
            profile.IpPortAddress = this.IpPortAddress;
            profile.NetPeer       = this.NetPeer;
            profile.PlayerName    = playerName;
            profile.UniqueId      = uniqueId;
            profile.PlayerId      = this.PlayerId;

            if (profile.InventoryItems == null)
            {
                profile.InventoryItems = StorageContainer.Create(6, 8);
            }
            
            return profile;
        }

        /**
         *
         * Oyuncu tamamen bağlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFullConnected()
        {
            this.IsFullConnected = true;

            try
            {
                PlayerFullConnectedEventArgs args = new PlayerFullConnectedEventArgs(this);

                Handlers.OnPlayerFullConnected(args);
            }
            catch (Exception e)
            {
                Log.Error($"AuthorizationProfile.OnFullConnected: {e}\n{e.StackTrace}");
            }
        }

        /**
         *
         * Oyuncu bağlantısı koptuğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisconnected()
        {
            this.IsFullConnected = false;

            try
            {
                PlayerDisconnectedEventArgs args = new PlayerDisconnectedEventArgs(this);

                Handlers.OnPlayerDisconnected(args);
            }
            catch (Exception e)
            {
                Log.Error($"AuthorizationProfile.OnDisconnected: {e}\n{e.StackTrace}");
            }

            Log.Info("DISCONNECT PLAYER -> " + this.PlayerName);

            Server.Instance.Logices.CreatureWatcher.OnPlayerDisconnected(this.PlayerId);
            Server.Instance.Logices.Bed.ClearPlayerBeds(this.PlayerId);
            Server.Instance.Logices.EntityWatcher.RemoveOwnershipByPlayer(this.UniqueId);
            Server.Instance.Logices.Hoverpad.RemovePlayerFromPlatform(this.UniqueId, true);
            Server.Instance.Logices.Interact.RemoveBlockByPlayerId(this.UniqueId);
            Server.Instance.Logices.PlayerJoin.OnPlayerDisconnected(this.UniqueId);
            Server.Instance.Logices.BaseMapRoom.OnPlayerDisconnected(this.UniqueId);
            Server.Instance.Logices.VoidLeviathan.OnPlayerDisconnected(this);

            this.SaveToDisk();

            ServerModel.PlayerDisconnectedArgs packet = new ServerModel.PlayerDisconnectedArgs()
            {
                UniqueId  = this.UniqueId
            };

            this.SendPacketToOtherClients(packet);
        }

        /**
         *
         * SubrootId Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddUsedTool(TechType techType)
        {
            if (!this.UsedTools.Contains(techType))
            {
                this.UsedTools.Add(techType);
            }
        }

        /**
         *
         * Biome döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetBiome()
        {
            return LargeWorld.main.GetBiome(this.Position.ToVector3());
        }

        /**
         *
         * Oyuncunun öle bölgede olma durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetInVoidBiome(bool isInVoidBiome)
        {
            this.IsInVoidBiome = isInVoidBiome;
        }

        /**
         *
         * Araç id numarasını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetVehicle(string vehicleId)
        {
            this.VehicleId = vehicleId;
        }

        /**
         *
         * SubrootId Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSubroot(string subrootId)
        {
            this.SubrootId = subrootId;
        }

        /**
         *
         * InteriorId Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetInterior(string interiorId)
        {
            this.InteriorId = interiorId;
        }

        /**
         *
         * RespawnPointId Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetRespawnPointId(string respawnPointId)
        {
            this.RespawnPointId = respawnPointId;
        }

        /**
         *
         * Oyuncu konumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPosition(ZeroVector3 position, ZeroQuaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }

        /**
         *
         * En son hipnoz zamanını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetLastHypnotizeTime(float lastHypnotizeTime)
        {
            this.LastHypnotizeTime = lastHypnotizeTime + 30f;
        }

        /**
         *
         * Envanter eşyalarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddInventoryItem(StorageItem item)
        {
            this.RemoveInventoryItem(item.ItemId);
            this.InventoryItems.AddItem(item);
            return true;
        }

        /**
         *
         * Envanter idlerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveInventoryItem(string itemId)
        {
            this.InventoryItems.RemoveItem(itemId);
        }

        /**
         *
         * Hedefi tamamlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CompleteGoal(string storyKey, GoalType goalType, bool isPlayMuted)
        {
            return this.SpecialGoals.Add(new ZeroStoryGoal()
            {
                Key          = storyKey,
                GoalType     = goalType,
                IsPlayMuted  = isPlayMuted,
                FinishedTime = Server.Instance.Logices.World.GetServerTime(),
            });
        }

        /**
         *
         * Hipnoz aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsHypnotized()
        {
            return this.LastHypnotizeTime > Server.Instance.Logices.World.GetServerTime();
        }

        /**
         *
         * Envanterde nesne mevcut mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInventoryItemExists(string uniqueId)
        {
            return this.InventoryItems.IsItemExists(uniqueId);
        }

        /**
         *
         * Kullanılan odayı değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUsingRoomId(string constructionId)
        {
            this.UsingRoomId = constructionId;
        }

        /**
         *
         * Oyuncu Saldırı altında durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUnderAttack(float attackTime)
        {
            this.LastAttackTime = Server.Instance.Logices.World.GetServerTime() + attackTime;
        }

        /**
         *
         * Oyuncu Saldırı altında mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUnderAttack()
        {
            return this.LastAttackTime > Server.Instance.Logices.World.GetServerTime();
        }

        /**
         *
         * Envanter eşyalarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetStoryCinematicMode(bool isActive)
        {
            this.IsStoryCinematicModeActive = isActive;
        }      

        /**
         *
         * Ekipman eşyalarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetEquipments(byte[] equipments, Dictionary<string, string> equipmentSlots)
        {
            this.Equipments = equipments;
            this.EquipmentSlots = equipmentSlots;
        }

        /**
         *
         * Hızlı slot id'lerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetQuickSlots(string[] slots)
        {
            this.QuickSlots = slots;
        }

        /**
         *
         * Aktif slot değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetActiveSlot(int activeSlot)
        {
            this.ActiveSlot = activeSlot;
        }

        /**
         *
         * Teknoloji pinlerini değiştir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPinItems(List<TechType> itemPins)
        {
            this.ItemPins = itemPins;
        }

        /**
         *
         * Hava durumu profilini değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetWeatherProfile(string profileId)
        {
            this.WeatherProfileId = profileId;
        }

        /**
         *
         * Bildirim siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveNotification(string key)
        {
            this.PdaNotifications.RemoveWhere(q => q.Key == key);
        }

        /**
         *
         * Bildirim ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddNotification(NotificationManager.Group group, string key, bool isAdded)
        {
            var notification = this.PdaNotifications.FirstOrDefault(q => q.Key == key);
            if (notification == null) 
            {
                this.PdaNotifications.Add(new NotificationItem(group, key, !isAdded, false, true, 0));
            }
            else
            {
                if (!notification.IsViewed && !isAdded)
                {
                    notification.IsViewed = true;
                }
            }
        }

        /**
         *
         * Bildirim ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetNotificationVisible(string uniqueId, bool isVisible)
        {
            var notification = this.PdaNotifications.FirstOrDefault(q => q.Key == uniqueId);
            if (notification == null)
            {
                this.PdaNotifications.Add(new NotificationItem(NotificationManager.Group.Undefined, uniqueId, true, true, isVisible, 0));
            }
            else 
            {
                notification.IsPing    = true;
                notification.IsVisible = isVisible;
            }

        }

        /**
         *
         * Bildirim ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetNotificationColorIndex(string uniqueId, sbyte colorIndex)
        {
            var notification = this.PdaNotifications.FirstOrDefault(q => q.Key == uniqueId);
            if (notification == null)
            {
                this.PdaNotifications.Add(new NotificationItem(NotificationManager.Group.Undefined, uniqueId, true, true, true, colorIndex));
            }
            else 
            {
                notification.IsPing     = true;
                notification.ColorIndex = colorIndex;
            }

        }

        /**
         *
         * Sağlığı değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetHealth(float health)
        {
            this.Health = health;
        }

        /**
         *
         * Yiyecek miktarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetFood(float food)
        {
            this.Food = food;
        }

        /**
         *
         * Su miktarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetWater(float water)
        {
            this.Water = water;
        }

        /**
         *
         * Balığı görebilir miyim?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CanSeeTheCreature(MultiplayerCreatureItem creature, bool longDistance = false)
        {
            return this.Position.Distance(creature.Position) < creature.Data.GetVisibilityDistance(longDistance);
        }

        /**
         *
         * Oyuncuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendPacket(NetworkPacket packet)
        {
            Server.SendPacket(this, packet);
        }

        /**
         *
         * Oyuncuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendPacketToAllClient(NetworkPacket packet, bool checkConnected = false)
        {
            Server.SendPacketToAllClient(packet, checkConnected);
        }

        /**
         *
         * Oyuncuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendPacketToOtherClients(NetworkPacket packet, bool checkConnected = false)
        {
            Server.SendPacketToOtherClients(this, packet, checkConnected);
        }

        /**
         *
         * Verileri diske yazar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SaveToDisk()
        {
            lock (Server.Instance.Storages.Player.ProcessLock)
            {
                var data = NetworkTools.Serialize(this);
                if (data == null)
                {
                    Log.Error(string.Format("Player.SaveToDisk -> Error Code (0x01): {0}", this.UniqueId));
                    return false;
                }

                if (!data.IsValid())
                {
                    Log.Error(string.Format("Player.SaveToDisk -> Error Code (0x02): {0}", this.UniqueId));
                    return false;
                }

                return data.WriteToDisk(Server.Instance.Storages.Player.GetPlayerFilePath(this.UniqueId));
            }
        }
    }
}

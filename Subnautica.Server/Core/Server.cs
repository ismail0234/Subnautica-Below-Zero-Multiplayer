namespace Subnautica.Server.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteNetLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using EventHandlers  = Subnautica.Events.Handlers;
    using ServerHandlers = Subnautica.Server.Events.Handlers;

    public class Server
    {
        /**
         *
         * Debug
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DEBUG { get; set; } = true;
                 
        /**
         *
         * Singleton nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Server Instance { get; set; }

        /**
         *
         * Sunucu Port
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int Port { get; set; }

        /**
         *
         * Max Oyuncu Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte MaxPlayer { get; set; }

        /**
         *
         * Oyunun şuanki oluşturulma durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsConnecting { get; set; }

        /**
         *
         * Oyunun oluşturulup oluşturulmadığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsConnected { get; set; }

        /**
         *
         * Sunucunun benzersiz numarası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ServerId { get; set; }

        /**
         *
         * Sunucunun sahibi id'si.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string OwnerId { get; set; }

        /**
         *
         * Sunucu kayıt dosya yolu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SavePath { get; set; }

        /**
         *
         * Mevcut playerId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte CurrentPlayerId { get; set; } = 0;

        /**
         *
         * Version değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Version { get; set; }

        /**
         *
         * GameMode değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameModePresetId GameMode { get; set; }

        /**
         *
         * Bağlı Oyuncular
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, AuthorizationProfile> Players { get; set; }

        /**
         *
         * Depolama işlemleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Storages Storages { get; set; }

        /**
         *
         * TCP Server
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private NetManager NetworkServer { get; set; }

        /**
         *
         * Sunucu Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private GameObject ServerGameObject { get; set; }

        /**
         *
         * IsRegisteredEvents Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsRegisteredEvents { get; set; } = false;

        /**
         *
         * Mantıksal işlemleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Logices Logices { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Server(string serverId, GameModePresetId gameModeId, int port, byte maxPlayer, string ownerId, string version)
        {
            Instance = this;

            this.Dispose();

            this.Port      = port;
            this.MaxPlayer = maxPlayer;
            this.ServerId  = serverId;
            this.OwnerId   = ownerId;
            this.Version   = version;
            this.GameMode  = gameModeId;
            this.SavePath  = Paths.GetMultiplayerServerSavePath(this.ServerId);
            this.Players   = new Dictionary<string, AuthorizationProfile>();

            this.ServerGameObject = new GameObject(serverId);

            this.Logices = this.ServerGameObject.AddComponent<Logices>();

            this.Storages = new Storages();
            this.Storages.Start(this.ServerId);

            this.RegisterEvents();
        }

        /**
         *
         * Sunucuyu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start()
        {
            try
            {
                this.IsConnecting = true;

                Log.Info("Starting Server...");
                Log.Info(String.Format("Server Port: {0}", this.Port));

                this.NetworkServer = new NetManager(new ServerListener())
                {
                    UpdateTime            = 1,
                    AutoRecycle           = true,
                    UnsyncedReceiveEvent  = true,
                    UnsyncedDeliveryEvent = true,
                    UnsyncedEvents        = true,
                    ChannelsCount         = Network.GetChannelCount(),
                    IPv6Enabled           = false,
                };

                this.NetworkServer.Start(this.Port);

                this.IsConnecting = false;
                this.IsConnected  = true;

                Log.Info("Started Server.");
                Log.Info("Waiting for players");
            }
            catch (Exception e)
            {
                this.IsConnecting = false;
                this.IsConnected  = false;
                Log.Error($"Server.Start Exception: {e}");
            }
        }

        /**
         *
         * Network server nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetManager GetNetworkServer()
        {
            return this.NetworkServer;
        }

        /**
         *
         * Bağlı peer sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetConnectedPeerCount()
        {
            return this.NetworkServer.ConnectedPeersCount;
        }

        /**
         *
         * Sonraki oyuncu id'sini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetNextPlayerId()
        {
            if (this.Players.Count > 250)
            {
                return 251;
            }

            while (true)
            {
                ++this.CurrentPlayerId;

                if (this.CurrentPlayerId > 250)
                {
                    this.CurrentPlayerId = 1;
                }

                if (this.HasPlayer(this.CurrentPlayerId))
                {
                    continue;
                }

                return this.CurrentPlayerId;
            }
        }

        /**
         *
         * Paketi tüm kullanıcılara gönderir fakat 1 ip'yi görmezden gelir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToOtherClients(AuthorizationProfile profile, NetworkPacket packet, bool checkConnected = false)
        {
            if (Server.Instance.IsLogablePacket(packet.Type))
            {
                Log.Info($"PACKET SENDED: [Length: {NetworkTools.Serialize(packet).Length}] -> {packet.Type}");
            }
            
            foreach (var player in Server.Instance.Players.Values.Where(q => q.IpPortAddress != profile.IpPortAddress))
            {
                if (checkConnected && !player.IsFullConnected)
                {
                    continue;
                }

                SendPacket(player.IpPortAddress, packet);
            }
        }

        /**
         *
         * Paketi tüm kullanıcılara gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToAllClient(NetworkPacket packet, bool checkConnected = false)
        {
            if (Server.Instance.IsLogablePacket(packet.Type))
            {
                Log.Info($"PACKET SENDED: [Length: {NetworkTools.Serialize(packet).Length}] -> {packet.Type}");
            }
            
            foreach (var player in Server.Instance.Players)
            {
                if (checkConnected && !player.Value.IsFullConnected)
                {
                    continue;
                }

                SendPacket(player.Value.IpPortAddress, packet);
            }
        }

        /**
         *
         * Paketi 1 kullanıcıya gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacket(AuthorizationProfile profile, NetworkPacket packet)
        {
            SendPacket(profile.IpPortAddress, packet);
        }

        /**
         *
         * Paketi 1 kullanıcıya byte halinde gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool SendPacket(string ipPort, NetworkPacket packet)
        {
            if (Server.Instance.Players.TryGetValue(ipPort, out var profile))
            {
                if (profile.NetPeer != null && profile.NetPeer.ConnectionState == ConnectionState.Connected)
                {
                    profile.NetPeer.Send(packet.Serialize(), packet.ChannelId, packet.DeliveryMethod);

                    Server.Instance.NetworkServer.TriggerUpdate();
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Bir kullanıcının bağlantısını keser.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DisconnectToClient(AuthorizationProfile authorization)
        {
            return DisconnectToClient(authorization.IpPortAddress);
        }

        /**
         *
         * Bir kullanıcının bağlantısını keser.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DisconnectToClient(string ipPort)
        {
            if (Server.Instance.NetworkServer != null)
            {
                foreach (var peer in Server.Instance.NetworkServer.ConnectedPeerList)
                {
                    if (peer.ToString() == ipPort)
                    {
                        peer.Disconnect();
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         *
         * Oyuncuları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<AuthorizationProfile> GetPlayers()
        {
            return this.Players.Values.ToList();
        }

        /**
         *
         * Oyuncu sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetPlayerCount()
        {
            return (byte) this.Players.Count;
        }

        /**
         *
         * Sunucu sahibini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile GetServerOwner()
        {
            return this.GetPlayer(this.OwnerId);
        }

        /**
         *
         * Oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile GetPlayer(byte playerId)
        {
            return this.Players.FirstOrDefault(q => q.Value.PlayerId == playerId).Value;
        }

        /**
         *
         * Oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile GetPlayer(string uniqueId)
        {
            return this.Players.FirstOrDefault(q => q.Value.UniqueId == uniqueId).Value;
        }

        /**
         *
         * Oyuncunun mevcut olup/olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasPlayer(string uniqueId)
        {
            return this.Players.ContainsKey(uniqueId);
        }

        /**
         *
         * Oyuncunun mevcut olup/olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasPlayer(byte playerId)
        {
            return this.Players.Any(q => q.Value.PlayerId == playerId);
        }

        /**
         *
         * Sunucu olaylarını kaydeder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void RegisterEvents()
        {
            if (!this.IsRegisteredEvents)
            {
                this.IsRegisteredEvents = true;

                EventHandlers.Game.PowerSourceAdding        += this.Logices.PowerConsumer.OnPowerSourceAdding;
                EventHandlers.Game.PowerSourceRemoving      += this.Logices.PowerConsumer.OnPowerSourceRemoving;
                EventHandlers.Game.EntityDistributionLoaded += this.Logices.WorldStreamer.OnEntityDistributionLoaded;

                EventHandlers.Building.BaseHullStrengthCrushing += this.Logices.BaseHullStrength.OnCrushing;

                ServerHandlers.PlayerFullConnected += this.Logices.CreatureWatcher.OnPlayerFullConnected;
                ServerHandlers.PlayerDisconnected  += this.Logices.ServerApi.OnPlayerDisconnected;

                MainGameController.OnGameStarted.AddHandler(new Action(this.Logices.PowerConsumer.OnGameStart));
            }
        }

        /**
         *
         * Sunucu olaylarını kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UnRegisterEvents()
        {
            if (this.IsRegisteredEvents)
            {
                this.IsRegisteredEvents = false;

                EventHandlers.Game.PowerSourceAdding        -= this.Logices.PowerConsumer.OnPowerSourceAdding;
                EventHandlers.Game.PowerSourceRemoving      -= this.Logices.PowerConsumer.OnPowerSourceRemoving;
                EventHandlers.Game.EntityDistributionLoaded -= this.Logices.WorldStreamer.OnEntityDistributionLoaded;

                EventHandlers.Building.BaseHullStrengthCrushing -= this.Logices.BaseHullStrength.OnCrushing;
                
                ServerHandlers.PlayerDisconnected  -= this.Logices.ServerApi.OnPlayerDisconnected;
                ServerHandlers.PlayerFullConnected -= this.Logices.CreatureWatcher.OnPlayerFullConnected;

                MainGameController.OnGameStarted.RemoveHandler(new Action(this.Logices.PowerConsumer.OnGameStart));
            }
        }

        /**
         *
         * Hariç tutulacak paket türleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLogablePacket(ProcessType type)
        {
            if (!Core.Server.DEBUG)
            {
                return false;
            }

            switch (type)
            {
                case ProcessType.PlayerUpdated:
                case ProcessType.PlayerStats:
                case ProcessType.WorldDynamicEntityPosition:
                case ProcessType.WorldCreaturePosition:
                case ProcessType.VehicleUpdated:
                case ProcessType.PlayerAnimationChanged: 
                case ProcessType.EnergyTransmission: 
                case ProcessType.VehicleEnergyTransmission: 
                case ProcessType.Ping: 
                case ProcessType.CreatureAnimation: 
                    return false;
            }

            return true;
        }

        /**
         *
         * Sınıfı temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose(bool isEndGame = false)
        {
            this.UnRegisterEvents();

            if (this.Logices != null)
            {
                this.Logices.AutoSave.SaveAll();
            }

            if (this.NetworkServer != null)
            {
                this.NetworkServer.DisconnectAll();
                this.NetworkServer.Stop();
                this.NetworkServer = null;
            }

            if (isEndGame)
            {
                this.Logices.StoryTrigger.ResetEndGame();
                this.Logices.AutoSave.SaveAll();
                
                foreach (var player in this.Storages.Player.GetAllPlayers())
                {
                    player.SetPosition(new ZeroVector3(-235.4f, 6.5f, 163.5f), new ZeroQuaternion(0.0f, -0.9f, 0.0f, 0.4f));
                    player.SaveToDisk();
                }
            }

            this.Port         = 0;
            this.MaxPlayer    = 0;
            this.IsConnecting = false;
            this.IsConnected  = false;
            this.ServerId     = null;
            this.OwnerId      = null;
            this.SavePath     = null;
            this.Players      = null;

            if (this.Storages != null)
            {
                this.Storages.Dispose();
                this.Storages = null;
            }

            if (this.ServerGameObject != null)
            {
                this.ServerGameObject.Destroy();
                this.ServerGameObject = null;
            }
        }
    }
}
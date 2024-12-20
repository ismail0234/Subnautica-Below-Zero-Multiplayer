namespace Subnautica.Client.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Subnautica.API.Features;

    public class NetworkServer
    {
        /**
         *
         * Varsayılan Local Sunucu Ip Adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string DefaultLocalIpAddress = "127.0.0.1";

        /**
         *
         * Varsayılan Sunucu Portu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const int DefaultPort = 666;

        /**
         *
         * Varsayılan Max Oyuncu Sayısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const int DefaultMaxPlayer = 8;

        /**
         *
         * Sunucuya bağlanılıyor mu durumu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsConnecting()
        {
            return Server.Core.Server.Instance != null && Server.Core.Server.Instance.IsConnecting;
        }

        /**
         *
         * Sunucuya bağlanıldı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsConnected()
        {
            return Server.Core.Server.Instance != null && Server.Core.Server.Instance.IsConnected;
        }

        /**
         *
         * Sunucu Id'si oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string CreateServerId()
        {
            List<HostServerItem> servers = GetHostServerList();

            while (true)
            {
                string serverId = Guid.NewGuid().ToString();

                if (!servers.Where(q => q.Id == serverId).Any())
                {
                    return serverId;
                }
            }
        }

        /**
         *
         * Yeni Sunucu Oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string CreateNewServer(GameModePresetId gameModeId)
        {
            var serverId   = CreateServerId();
            var serverPath = Paths.GetMultiplayerServerSavePath(serverId, "config.json");

            Directory.CreateDirectory(Path.GetDirectoryName(serverPath));

            HostServerItem serverItem = new HostServerItem()
            {
                GameMode       = (int) gameModeId,
                CreationDate   = Tools.GetUnixTime(),
                LastPlayedDate = Tools.GetUnixTime(),
            };

            File.WriteAllText(serverPath, Newtonsoft.Json.JsonConvert.SerializeObject(serverItem));
            return serverId;
        }

        /**
         *
         * Sunucuyu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool StartServer(string serverId, string ownerId)
        {
            var data = GetHostServerList().FirstOrDefault(q => q.Id == serverId);
            if (data == null)
            {
                return false;
            }

            try
            {
                NetworkServer.AbortServer();

                Server.Core.Server server = new Server.Core.Server(data.Id, data.GetGameMode(), NetworkServer.DefaultPort, NetworkServer.DefaultMaxPlayer, Tools.CreateMD5(ownerId), Tools.GetLauncherVersion());
                server.Start();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return true;
        }

        /**
         *
         * Sunucu kapatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void AbortServer(bool isEndGame = false)
        {
            Network.Session.Dispose();

            if (Server.Core.Server.Instance != null)
            {
                Server.Core.Server.Instance.Dispose(isEndGame);
                Server.Core.Server.Instance = null;
            }
        }

        /**
         *
         * Hızlı şekilde yapıları senkronlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void UpdateConstructionSync(byte[] constructionData)
        {
            if (NetworkServer.IsConnected())
            {
                Server.Core.Server.Instance.Storages.World.UpdateConstructions(constructionData);
            }
        }

        /**
         *
         * Local Sunucu Listesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<LocalServerItem> GetLocalServerList()
        {
            var serverListPath = Paths.GetGameServersPath();
            if (File.Exists(serverListPath))
            {
                var servers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocalServerItem>>(File.ReadAllText(serverListPath));
                if (servers == null)
                {
                    return new List<LocalServerItem>();
                }

                return servers;
            }

            SaveLocalServerList(new List<LocalServerItem>());
            return GetLocalServerList();
        }

        /**
         *
         * Local Sunucu Listesini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SaveLocalServerList(List<LocalServerItem> serverList)
        {
            try
            {
                File.WriteAllText(Paths.GetGameServersPath(), Newtonsoft.Json.JsonConvert.SerializeObject(serverList));
            }
            catch (Exception e)
            {
                Log.Error($"NetworkServer.SaveLocalServerList Exception: {e}");
            }
        }

        /**
         *
         * Barındırılan Sunucu Listesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<HostServerItem> GetHostServerList()
        {
            string serverPath = Paths.GetMultiplayerServerSavePath();

            Directory.CreateDirectory(serverPath);

            List<HostServerItem> servers = new List<HostServerItem>();

            foreach (var path in Directory.GetDirectories(serverPath))
            {
                string serverId = Path.GetFileName(path);
                if (string.IsNullOrEmpty(serverId))
                {
                    continue;
                }

                try
                {
                    string serverConfigPath = Paths.GetMultiplayerServerSavePath(serverId, "config.json");

                    if (File.Exists(serverConfigPath))
                    {
                        HostServerItem server = Newtonsoft.Json.JsonConvert.DeserializeObject<HostServerItem>(File.ReadAllText(serverConfigPath));
                        server.Id = serverId;

                        servers.Add(server);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"NetworkServer.GetHostServerList Exception: {e}");
                }
            }

            return servers;
        }
    }
}

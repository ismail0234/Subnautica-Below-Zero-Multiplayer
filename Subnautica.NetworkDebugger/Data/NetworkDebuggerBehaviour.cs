namespace Subnautica.NetworkDebugger.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteNetLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Client.Core;

    using UnityEngine;

    public class NetworkDebuggerBehaviour : MonoBehaviour
    {
        /**
         *
         * Instance Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static NetworkDebuggerBehaviour main;

        /**
         *
         * Instance Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static NetworkDebuggerBehaviour Instance
        {
            get
            {
                if (NetworkDebuggerBehaviour.main == null)
                {
                    NetworkDebuggerBehaviour.main = new GameObject().AddComponent<NetworkDebuggerBehaviour>();
                    NetworkDebuggerBehaviour.main.gameObject.SetActive(false);
                }

                return NetworkDebuggerBehaviour.main;
            }
        }

        /**
         *
         * IsActive Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive { get; set; }

        /**
         *
         * Timing Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * KeyValues Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();

        /**
         *
         * PacketLogs Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<NetworkDebuggerPacketLogItem> PacketLogs = new List<NetworkDebuggerPacketLogItem>();

        /**
         *
         * Basic Butonuna tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Menu_BasicIsActive { get; set; }

        /**
         *
         * General Butonuna tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Menu_GeneralIsActive { get; set; }

        /**
         *
         * Client Butonuna tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Menu_ClientIsActive { get; set; }

        /**
         *
         * Server Butonuna tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Menu_ServerIsActive { get; set; }

        /**
         *
         * Paketi loglar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddPacketLog(int size, NetworkChannel chanelType, DeliveryMethod deliveryMethod, bool isDownload, bool isClient)
        {
            this.PacketLogs.Add(new NetworkDebuggerPacketLogItem()
            {
                Size           = size,
                DeliveryMethod = deliveryMethod,
                Channel        = chanelType,
                IsDownload     = isDownload,
                IsClient       = isClient,
            });
        }

        /**
         *
         * Gösterme işlemini sağlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Show()
        {
            NetworkDebuggerBehaviour.Instance.gameObject.SetActive(true);
        }

        /**
         *
         * Gizleme işlemini sağlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Hide()
        {
            NetworkDebuggerBehaviour.Instance.gameObject.SetActive(false);
        }

        /**
         *
         * İstatistikleri etkinleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            this.Timing.Restart();
            this.KeyValues.Clear();
            this.PacketLogs.Clear();
            this.ResetActiveTabs();
            this.Menu_BasicIsActive = true;
            this.IsActive = true;

            NetworkClient.Client.EnableStatistics = true;
            NetworkClient.Client.Statistics.Reset();

            if (NetworkServer.IsConnected())
            {
                Server.Core.Server.Instance.GetNetworkServer().EnableStatistics = true;
                Server.Core.Server.Instance.GetNetworkServer().Statistics.Reset();
            }
        }

        /**
         *
         * İstatistikleri pasifleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.Timing.Stop();
            this.KeyValues.Clear();
            this.PacketLogs.Clear();
            this.IsActive = false;

            NetworkClient.Client.EnableStatistics = false;
            NetworkClient.Client.Statistics.Reset();

            if (NetworkServer.IsConnected())
            {
                Server.Core.Server.Instance.GetNetworkServer().EnableStatistics = false;
                Server.Core.Server.Instance.GetNetworkServer().Statistics.Reset();
            }
        }

        /**
         *
         * Üst menüyü oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CreateTopMenus()
        {
            GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());

            if (GUILayout.Button("Basic", Array.Empty<GUILayoutOption>()))
            {
                this.ResetActiveTabs();
                this.Menu_BasicIsActive = true;
            }

            if (GUILayout.Button("General", Array.Empty<GUILayoutOption>()))
            {
                this.ResetActiveTabs();
                this.Menu_GeneralIsActive = true;
            }

            if (GUILayout.Button("Client", Array.Empty<GUILayoutOption>()))
            {
                this.ResetActiveTabs();
                this.Menu_ClientIsActive = true;
            }

            if (NetworkServer.IsConnected())
            {
                if (GUILayout.Button("Server", Array.Empty<GUILayoutOption>()))
                {
                    this.ResetActiveTabs();
                    this.Menu_ServerIsActive = true;
                }
            }

            GUILayout.EndHorizontal();
        }

        /**
         *
         * Client içeriğini oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CreateClientContent(bool isBasic = false)
        {
            GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
            GUILayout.Label("Client Network Statistics", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.Label($"Upload: {this.GetOption<double>("ClientUpload")}KB/s\nDownload: {this.GetOption<double>("ClientDownload")}KB/s\nPacketLoss: {this.GetOption<double>("ClientPacketLoss")} ({this.GetOption<double>("ClientPacketLossPercent")}%)", Array.Empty<GUILayoutOption>());

            if (isBasic == false)
            {
                var downloadChannelLogs = this.GetOption<string>("ClientDownloadChannelStats");
                if (downloadChannelLogs.IsNotNull())
                {
                    GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
                    GUILayout.Label("Client Channel Statistics (Download)", Array.Empty<GUILayoutOption>());
                    GUILayout.EndHorizontal();

                    GUILayout.Label(downloadChannelLogs, Array.Empty<GUILayoutOption>());
                }

                var uploadChannelLogs = this.GetOption<string>("ClientUploadChannelStats");
                if (uploadChannelLogs.IsNotNull())
                {
                    GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
                    GUILayout.Label("Client Channel Statistics (Upload)", Array.Empty<GUILayoutOption>());
                    GUILayout.EndHorizontal();

                    GUILayout.Label(uploadChannelLogs, Array.Empty<GUILayoutOption>());
                }
            }
        }

        /**
         *
         * Server içeriğini oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CreateServerContent(bool isBasic = false)
        {
            if (NetworkServer.IsConnected())
            {
                GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
                GUILayout.Label("Server Network Statistics", Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
                GUILayout.Label($"Received: {this.GetOption<double>("ServerUpload")}KB/s\nSended: {this.GetOption<double>("ServerDownload")}KB/s\nPacketLoss: {this.GetOption<double>("ServerPacketLoss")} ({this.GetOption<double>("ServerPacketLossPercent")}%)", Array.Empty<GUILayoutOption>());

                if (isBasic == false)
                {
                    var downloadChannelLogs = this.GetOption<string>("ServerDownloadChannelStats");
                    if (downloadChannelLogs.IsNotNull())
                    {
                        GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
                        GUILayout.Label("Server Channel Statistics (Sended)", Array.Empty<GUILayoutOption>());
                        GUILayout.EndHorizontal();

                        GUILayout.Label(downloadChannelLogs, Array.Empty<GUILayoutOption>());
                    }

                    var uploadChannelLogs = this.GetOption<string>("ServerUploadChannelStats");
                    if (uploadChannelLogs.IsNotNull())
                    {
                        GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
                        GUILayout.Label("Server Channel Statistics (Received)", Array.Empty<GUILayoutOption>());
                        GUILayout.EndHorizontal();

                        GUILayout.Label(uploadChannelLogs, Array.Empty<GUILayoutOption>());
                    }
                }
            }
        }

        /**
         *
         * İçeriği çizer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20f, 20f, (float)Screen.width * 0.4f, (float)Screen.height));
            GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());

            this.CreateTopMenus();

            if (this.Menu_BasicIsActive)
            {
                this.CreateClientContent(true);
                this.CreateServerContent(true);
            }

            if (this.Menu_GeneralIsActive)
            {
                this.CreateClientContent();
                this.CreateServerContent();
            }

            if (this.Menu_ClientIsActive)
            {
                this.CreateClientContent();
            }

            if (this.Menu_ServerIsActive)
            {
                this.CreateServerContent();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.Timing.IsFinished())
            {
                this.Timing.Restart();

                this.SetOption("ClientUpload", System.Math.Round((double)NetworkClient.Client.Statistics.BytesSent / 1024, 2));
                this.SetOption("ClientDownload", System.Math.Round((double)NetworkClient.Client.Statistics.BytesReceived / 1024, 2));
                this.SetOption("ClientPacketLoss", System.Math.Round((double)NetworkClient.Client.Statistics.PacketLoss, 2));
                this.SetOption("ClientPacketLossPercent", System.Math.Round((double)NetworkClient.Client.Statistics.PacketLossPercent, 2));

                NetworkClient.Client.Statistics.Reset();

                if (NetworkServer.IsConnected())
                {
                    this.SetOption("ServerUpload", System.Math.Round((double)Server.Core.Server.Instance.GetNetworkServer().Statistics.BytesSent / 1024, 2));
                    this.SetOption("ServerDownload", System.Math.Round((double)Server.Core.Server.Instance.GetNetworkServer().Statistics.BytesReceived / 1024, 2));
                    this.SetOption("ServerPacketLoss", System.Math.Round((double)Server.Core.Server.Instance.GetNetworkServer().Statistics.PacketLoss, 2));
                    this.SetOption("ServerPacketLossPercent", System.Math.Round((double)Server.Core.Server.Instance.GetNetworkServer().Statistics.PacketLossPercent, 2));

                    Server.Core.Server.Instance.GetNetworkServer().Statistics.Reset();
                }

                this.CacheClientChannelPacketLogs();
                this.CacheServerChannelPacketLogs();
                this.PacketLogs.Clear();
            }
        }

        /**
         *
         * Client Verilerini önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CacheClientChannelPacketLogs()
        {
            var downloads = new List<string>();
            var uploads   = new List<string>();

            foreach (var items in this.PacketLogs.Where(q => q.IsClient && q.IsDownload).GroupBy(q => q.Channel).Select(q => q.OrderBy(x => x.Channel).ToList()))
            {
                var totalSize = items.Sum(q => q.Size);

                downloads.Add($"{items.ElementAt(0).Channel}: {System.Math.Round((double)totalSize / 1024, 2)}KB/s ({items.Count})");
            }

            foreach (var items in this.PacketLogs.Where(q => q.IsClient && !q.IsDownload).GroupBy(q => q.Channel).Select(q => q.OrderBy(x => x.Channel).ToList()))
            {
                var totalSize = items.Sum(q => q.Size);

                uploads.Add($"{items.ElementAt(0).Channel}: {System.Math.Round((double)totalSize / 1024, 2)}KB/s ({items.Count})");
            }

            this.SetOption("ClientDownloadChannelStats", string.Join("\n", downloads));
            this.SetOption("ClientUploadChannelStats", string.Join("\n", uploads));
        }

        /**
         *
         * Server Verilerini önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CacheServerChannelPacketLogs()
        {
            var downloads = new List<string>();
            var uploads = new List<string>();

            foreach (var items in this.PacketLogs.Where(q => !q.IsClient && q.IsDownload).GroupBy(q => q.Channel).Select(q => q.OrderBy(x => x.Channel).ToList()))
            {
                var totalSize = items.Sum(q => q.Size);

                downloads.Add($"{items.ElementAt(0).Channel}: {System.Math.Round((double)totalSize / 1024, 2)}KB/s ({items.Count})");
            }

            foreach (var items in this.PacketLogs.Where(q => !q.IsClient && !q.IsDownload).GroupBy(q => q.Channel).Select(q => q.OrderBy(x => x.Channel).ToList()))
            {
                var totalSize = items.Sum(q => q.Size);

                uploads.Add($"{items.ElementAt(0).Channel}: {System.Math.Round((double)totalSize / 1024, 2)}KB/s ({items.Count})");
            }

            this.SetOption("ServerDownloadChannelStats", string.Join("\n", downloads));
            this.SetOption("ServerUploadChannelStats", string.Join("\n", uploads));
        }

        /**
         *
         * Aktif tabları sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ResetActiveTabs()
        {
            this.Menu_BasicIsActive = false;
            this.Menu_GeneralIsActive = false;
            this.Menu_ClientIsActive = false;
            this.Menu_ServerIsActive = false;
        }

        /**
         *
         * Değeri günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetOption(string key, object value)
        {
            this.KeyValues[key] = value;
        }

        /**
         *
         * Değeri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetOption<T>(string key)
        {
            if (this.KeyValues.TryGetValue(key, out var data))
            {
                return (T)data;

            }

            return default;
        }
    }
}

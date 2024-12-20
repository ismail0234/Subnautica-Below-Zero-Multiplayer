namespace Subnautica.Client.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using LiteNetLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Network.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Server;

    using UnityEngine;

    public class NetworkClient
    {
        /**
         *
         * En son bağlanılan Ip Adresi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string IpAddress { get; set; }

        /**
         *
         * En son bağlanılan Port
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int PortNumber { get; set; }

        /**
         *
         * Bağlanma durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsConnectingToServer { get; set; } = false;

        /**
         *
         * Bağlantı durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsConnectedToServer { get; set; } = false;

        /**
         *
         * Güvenli bağlantı kesme durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSafeDisconnecting { get; set; } = false;

        /**
         *
         * Client Bağlantısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static NetManager Client { get; set; }

        /**
         *
         * Bağlantı Sinyalleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Queue<ConnectionSignal> ConnectionSignalDataQueues { get; set; } = new Queue<ConnectionSignal>();

        /**
         *
         * Sunucuya Bağlanır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Connect(string ipAddress, int port, bool officialServerConnect = true, bool retryConnect = true)
        {
            if (IsConnectedToServer || IsConnectingToServer)
            {
                return;
            }

            NetworkClient.IpAddress  = ipAddress;
            NetworkClient.PortNumber = port;

            try
            {
                NetworkClient.IsConnectingToServer = true;
                NetworkClient.IsConnectedToServer  = false;
                NetworkClient.IsSafeDisconnecting  = false;
                NetworkClient.ClearSignals();

                if (Client == null)
                {
                    Client = new NetManager(new ClientListener())
                    {
                        UpdateTime            = 1,
                        AutoRecycle           = true,
                        UnsyncedReceiveEvent  = true,
                        UnsyncedDeliveryEvent = true,
                        UnsyncedEvents        = true,
                        ChannelsCount         = Network.GetChannelCount(),
                        IPv6Enabled           = false,
                    };
                }

                if (Client.Start())
                {
                    Client.Connect(ipAddress, port, Tools.GetLauncherVersion());
                }
                else
                {
                    Log.Error($"NetworkClient.Connect.Start: NOT POSSIBLE");
                }
            }
            catch (TimeoutException e)
            {
                Log.Error($"NetworkClient.Connect.TimeoutException: {e}");
            }
            catch (Exception e)
            {
                Log.Error($"NetworkClient.Connect.Exception: {e}");
            }

            if (retryConnect)
            {
                NetworkClient.CheckConnectionAndJoinServer(officialServerConnect);
            }
        }

        /**
         *
         * Sunucuya bağlantı durumunu kontrol eder ve katılma isteği gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CheckConnectionAndJoinServer(bool officialServerConnect)
        {
            UWE.CoroutineHost.StartCoroutine(SubCheckConnectionAndJoinServer(officialServerConnect));
        }

        /**
         *
         * Sunucuya bağlantı durumunu kontrol eder ve katılma isteği gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator SubCheckConnectionAndJoinServer(bool officialServerConnect)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            int maxWait  = officialServerConnect ? 50 : 10;
            int maxRetry = officialServerConnect ? 3  : 1;

            for (int j = 0; j < maxRetry; j++)
            {
                Log.Info($"Connection Trying ({j + 1}) ");

                for (int i = 0; i < maxWait; i++)
                {
                    yield return new WaitForSecondsRealtime(0.1f);

                    ConnectionSignals.ConsumeQueue();

                    Log.Info($"WAIT ({i}): " + NetworkClient.IsConnected());

                    if (NetworkClient.IsConnected())
                    {
                        NetworkClient.JoinServer(Tools.GetLoggedInName(), Tools.GetLoggedId());
                        break;
                    }
                    else if (NetworkClient.IsDisconnected())
                    {
                        NetworkClient.ClearSignals();
                        break;
                    }
                }

                if (NetworkClient.IsConnected() || NetworkClient.IsDisconnected() || maxRetry == j + 1)
                {
                    break;
                }

                if (officialServerConnect)
                {
                    NetworkClient.IsSafeDisconnecting = true;
                    NetworkClient.Disconnect();

                    yield return new WaitForSecondsRealtime(1f);

                    NetworkClient.IsSafeDisconnecting = false;
                    NetworkClient.Connect(NetworkClient.IpAddress, NetworkClient.PortNumber, officialServerConnect, false);
                }
            }

            if (!NetworkClient.IsConnectedToServer && NetworkClient.IsConnectingToServer)
            {
                ErrorMessage.AddMessage(API.Features.ZeroLanguage.Get("GAME_SERVER_CONNECTING_ERROR"));
                NetworkClient.Disconnect();

                ZeroGame.StopLoadingScreen();
            }
        }
        
        /**
         *
         * Bağlanılıp/Bağlanılmadığını kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsConnected()
        {
            if (NetworkClient.Client == null || NetworkClient.IsConnectingToServer)
            {
                return false;
            }

            return NetworkClient.IsConnectedToServer;
        }

        /**
         *
         * Bağlantı tamamen kesildi mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsDisconnected()
        {
            return NetworkClient.Client == null;
        }

        /**
         *
         * Sunucuya Bağlantısını keser.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool Disconnect(bool isEndGame = false)
        {
            NetworkServer.AbortServer(isEndGame);

            IsConnectingToServer = false;
            IsConnectedToServer  = false;
            
            if (Client == null)
            {
                return false;
            }

            Client.DisconnectAll();
            Client.Stop();
            Client = null;
            return true;
        }

        /**
         *
         * Sunucu'ya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool SendPacket(NetworkPacket packet)
        {
            if (IsConnected() && !EventBlocker.IsEventBlocked(packet.Type))
            {
                NetworkClient.Client.SendToAll(packet.Serialize(), packet.ChannelId, packet.DeliveryMethod);
                return true;
            }

            return false;
        }

        /**
         *
         * Sunucuya Katılır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void JoinServer(string username, string userId)
        {
            try
            {
                MultiplayerChannelProcessor.AddToPlayerMultiplayerProcessors();

                JoiningServerArgs packet = new JoiningServerArgs()
                {
                    UserName = username,
                    UserId   = userId
                };

                NetworkClient.SendPacket(packet);
            }
            catch (Exception ex)
            {
                Log.Error($"NetworkClient.JoinServer - Exception: {ex}");
            }
        }

        /**
         *
         * Sinyalleri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ClearSignals()
        {
            ConnectionSignalDataQueues.Clear();
        }
    }
}

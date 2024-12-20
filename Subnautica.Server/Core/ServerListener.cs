namespace Subnautica.Server.Core
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using LiteNetLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Extensions;
    using Subnautica.Network.Models.Client;
    using Subnautica.Server.Abstracts;

    public class ServerListener : INetEventListener
    {
        /**
         *
         * Oyuncu bağlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPeerConnected(NetPeer peer)
        {
            Log.Info($"[{peer}] client connected...");
        }

        /**
         *
         * Bağlantı isteği gönderildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (Core.Server.Instance.GetConnectedPeerCount() < Core.Server.Instance.MaxPlayer)
            {
                var version = request.Data.GetString();
                Log.Info("V1: " + version + ", V2: " + Core.Server.Instance.Version);
                if (version == Core.Server.Instance.Version)
                {
                    request.Accept();
                }
                else
                {
                    request.Reject(this.GetRejectPacket(ConnectionSignal.VersionMismatch));
                }
            }
            else
            {
                request.Reject(this.GetRejectPacket(ConnectionSignal.ServerFull));
            }
        }

        /**
         *
         * Oyuncu bağlantısı kesildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Log.Info($"[{peer}] client disconnected: {disconnectInfo.Reason}");

            if (Core.Server.Instance.Players.TryGetValue(peer.ToString(), out var profile))
            {
                profile.OnDisconnected();

                Core.Server.Instance.Players.Remove(peer.ToString());
            }
        }

        /**
         *
         * Veri alındığında tetiklenir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            var packetSize = reader.AvailableBytes;

            try
            {
                var packet = reader.GetPacket();

                if (Server.Instance.IsLogablePacket(packet.Type))
                {
                    Log.Info($"PACKET RECEIVED: [Length: {packetSize}] -> {packet.Type}");
                }

                if (ProcessorShared.Processors.TryGetValue(packet.Type, out var processor))
                {
                    if (packet.Type == ProcessType.JoiningServer)
                    {
                        processor.OnExecute(new AuthorizationProfile(peer), packet);
                    }
                    else
                    {
                        if (Server.Instance.Players.TryGetValue(peer.ToString(), out var authorization))
                        {
                            if (authorization.IsAuthorized)
                            {
                                packet.SetPacketOwnerId(authorization.PlayerId);

                                processor.OnExecute(authorization, packet);
                            }
                            else
                            {
                                Server.DisconnectToClient(peer.ToString());
                                Log.Error($"[{packet.Type}] Player Not Authorized");
                            }
                        }
                        else
                        {
                            Server.DisconnectToClient(peer.ToString());
                            Log.Error($"[{packet.Type}] Player Not Found");
                        }
                    }
                }
                else
                {
                    // Server.DisconnectToClient(e.IpPort);
                    Log.Error($"[{packet.Type}] Processor Not Found: " + packet.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[Length: {packetSize}] => [{ex}]:");
            }
        }

        /**
         *
         * Ağ hatası olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            
        }

        /**
         *
         * Bağlanmamış kullanıcıdan veri geldiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        /**
         *
         * Ağ gecikmesi güncelleme durumunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        /**
         *
         * Reddetme paketini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte[] GetRejectPacket(ConnectionSignal rejectType)
        {
            var request = new ConnectionRejectArgs()
            {
                RejectType = rejectType
            };

            return request.Serialize();
        }
    }
}

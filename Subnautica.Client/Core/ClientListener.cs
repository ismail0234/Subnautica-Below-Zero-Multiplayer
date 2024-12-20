namespace Subnautica.Client.Core
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using LiteNetLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Network.Extensions;
    using Subnautica.Network.Models.Client;

    public class ClientListener : INetEventListener
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
            NetworkClient.IsConnectedToServer = true;
            NetworkClient.IsConnectingToServer = false;

            NetworkClient.ConnectionSignalDataQueues.Enqueue(ConnectionSignal.Connected);
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
            NetworkClient.IsConnectedToServer = false;
            NetworkClient.IsConnectingToServer = false;

            var rejectType = this.GetRejectType(disconnectInfo);
            if (rejectType == ConnectionSignal.ServerFull)
            {
                NetworkClient.ConnectionSignalDataQueues.Enqueue(ConnectionSignal.ServerFull);
            }
            else if (rejectType == ConnectionSignal.VersionMismatch)
            {
                NetworkClient.ConnectionSignalDataQueues.Enqueue(ConnectionSignal.VersionMismatch);
            }
            else
            {
                if (disconnectInfo.Reason == DisconnectReason.ConnectionRejected)
                {
                    NetworkClient.ConnectionSignalDataQueues.Enqueue(ConnectionSignal.Rejected);
                }
                else if (!NetworkClient.IsSafeDisconnecting)
                {
                    Log.Info("OnPeerDisconnected - Reason: " + disconnectInfo.Reason);
                    NetworkClient.ConnectionSignalDataQueues.Enqueue(ConnectionSignal.Disconnected);
                }
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
            try
            {
                var packet = reader.GetPacket();

                if (MultiplayerChannelProcessor.Processors.TryGetValue(packet.ChannelType, out var processor))
                {
                    processor.AddPacket(packet);
                }
                else
                {
                    Log.Error(string.Format("[OnClientDataReceived.NetworkReceiveEvent] Channel Not Found: {0}", packet.ChannelType));
                }
            }
            catch (Exception e)
            {
                Log.Error($"[OnClientDataReceived.NetworkReceiveEvent] Exception: {e}");
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
         * Reddetme türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ConnectionSignal GetRejectType(DisconnectInfo disconnectInfo)
        {
            try
            {
                if (disconnectInfo.AdditionalData.AvailableBytes > 0)
                {
                    var packet = disconnectInfo.AdditionalData.GetPacket()?.GetPacket<ConnectionRejectArgs>();
                    if (packet != null)
                    {
                        return packet.RejectType;
                    }
                }
            }
            catch (Exception)
            {
            
            }

            return ConnectionSignal.Unknown;
        }
    }
}

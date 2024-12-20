namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FreezeProcessor : NormalProcessor
    {
        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.PlayerFreezeArgs>();
            if (packet == null)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player != null)
            {
                if (packet.IsFreeze)
                {
                    player.EnableFreeze();
                }
                else
                {
                    player.DisableFreeze();
                }
            }

            return true;
        }

        /**
         *
         * Oyuncu donduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerFreezed(PlayerFreezedEventArgs ev)
        {
            FreezeProcessor.SendPacketToServer(true, ev.EndTime);
        }

        /**
         *
         * Oyuncu donma sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerUnfreezed()
        {
            FreezeProcessor.SendPacketToServer(false);
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(bool isFreeze, float endTime = -1f)
        {
            ServerModel.PlayerFreezeArgs request = new ServerModel.PlayerFreezeArgs()
            {
                IsFreeze = isFreeze,
                EndTime  = endTime
            };

            NetworkClient.SendPacket(request);
        }
    }
}
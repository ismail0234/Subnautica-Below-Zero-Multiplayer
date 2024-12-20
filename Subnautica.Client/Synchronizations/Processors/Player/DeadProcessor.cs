namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class DeadProcessor : NormalProcessor
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
            var player = ZeroPlayer.GetPlayerById(networkPacket.GetPacketOwnerId());
            if (player != null)
            {
                player.PingInstance.SetVisible(false);
                player.Hide(false);
            }

            return true;
        }

        /**
         *
         * Oyuncu öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerDead(PlayerDeadEventArgs ev)
        {
            ServerModel.PlayerDeadArgs request = new ServerModel.PlayerDeadArgs()
            {
                DamageType = ev.DamageType,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SpawnProcessor : NormalProcessor
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
                player.Animator.Rebind();
                player.PingInstance.SetVisible(true);
                player.Show(false);
            }

            return true;
        }

        /**
         *
         * Oyuncu doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerSpawned()
        {
            NetworkClient.SendPacket(new ServerModel.PlayerSpawnArgs());
        }
    }
}
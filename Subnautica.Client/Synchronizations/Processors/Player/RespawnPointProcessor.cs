namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class RespawnPointProcessor : NormalProcessor
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
            return true;
        }

        /**
         *
         * Oyuncunun yeniden doğma noktası değişince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerRespawnPointChanged(PlayerRespawnPointChangedEventArgs ev)
        {
            if (World.IsLoaded)
            {
                ServerModel.PlayerRespawnPointArgs result = new ServerModel.PlayerRespawnPointArgs()
                {
                    UniqueId = ev.UniqueId
                };

                NetworkClient.SendPacket(result);
            }
        }
    }
}

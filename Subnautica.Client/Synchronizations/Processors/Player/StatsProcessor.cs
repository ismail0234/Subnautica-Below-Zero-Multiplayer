namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using System.Diagnostics;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Modules;
    using Subnautica.Client.Synchronizations.InitialSync;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class StatsProcessor : NormalProcessor
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
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerStatsUpdated(PlayerStatsUpdatedEventArgs ev)
        {
            if (World.IsLoaded)
            {
                ServerModel.PlayerStatsArgs request = new ServerModel.PlayerStatsArgs()
                {
                    Health = ev.Health,
                    Food   = ev.Food,
                    Water  = ev.Water,
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}

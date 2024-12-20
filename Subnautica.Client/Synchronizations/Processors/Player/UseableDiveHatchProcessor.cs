namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class UseableDiveHatchProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.UseableDiveHatchArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player != null && player.IsMine)
            {
                player.OnHandClickUseableDiveHatch(packet.UniqueId, packet.IsEnter);
            }
            else
            {
                if (packet.IsMoonpoolExpansion == false)
                {
                    if (packet.IsEnter)
                    {
                        player.EnterStartCinematicUseableDiveHatch(packet.UniqueId);
                    }
                    else
                    {
                        player.ExitStartCinematicUseableDiveHatch(packet.UniqueId);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Oyuncu bir araca binmeye yada inmeye çalıştığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUseableDiveHatchClicking(UseableDiveHatchClickingEventArgs ev)
        {
            if (ev.IsBulkHead || ev.IsLifePod)
            {
                ev.IsAllowed = false;

                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    ServerModel.UseableDiveHatchArgs request = new ServerModel.UseableDiveHatchArgs()
                    {
                        UniqueId            = ev.UniqueId,
                        IsBulkHead          = ev.IsBulkHead,
                        IsLifePod           = ev.IsLifePod,
                        IsEnter             = ev.IsEnter,
                        IsMoonpoolExpansion = ev.IsMoonpoolExpansion,
                    };

                    NetworkClient.SendPacket(request);
                }               
            }
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.MonoBehaviours;
    using Subnautica.Client.MonoBehaviours.Player;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class InteriorProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleInteriorArgs>();
            if (packet.VehicleId.IsNull())
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player != null)
            {
                if (packet.IsEntered)
                {
                    player.SetInteriorId(packet.VehicleId);
                }
                else
                {
                    player.SetInteriorId(null);
                }

                player.GetComponent<PlayerAnimation>().FixedUpdate();
            }

            return true;
        }

        /**
         *
         * Araca binerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleInteriorToggle(VehicleInteriorToggleEventArgs ev)
        {
            ServerModel.VehicleInteriorArgs request = new ServerModel.VehicleInteriorArgs()
            {
                VehicleId = ev.UniqueId,
                IsEntered = ev.IsEnter,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
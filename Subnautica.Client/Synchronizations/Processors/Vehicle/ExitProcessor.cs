namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ExitProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleExitArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var entity = Network.DynamicEntity.GetEntity(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }

            entity.IsUsingByPlayer = false;
            entity.Position = packet.Position;
            entity.Rotation = packet.Rotation;

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player != null && !player.IsMine)
            {
                player.ExitVehicle();
            }
            
            return true;
        }

        /**
         *
         * Araçtan inerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleExited(VehicleExitedEventArgs ev)
        {
            if (Interact.IsBlockedByMe(ev.UniqueId))
            {
                ServerModel.VehicleExitArgs request = new ServerModel.VehicleExitArgs()
                {
                    UniqueId = ev.UniqueId,
                    TechType = ev.TechType,
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}
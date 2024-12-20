namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

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
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.VehicleExitArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId))
            {
                return false;
            }

            var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }

            if (entity.TechType.IsSeaTruckModule(false))
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.SeaTruckModuleEndPilot);
            }
            else if (entity.TechType == TechType.SeaTruck)
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.SeaTruckEndPilot);
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);
            }

            entity.IsUsingByPlayer = false;

            packet.Position = entity.Position;
            packet.Rotation = entity.Rotation;

            profile.SetVehicle(null);
            profile.SetUsingRoomId(null);       
            profile.SendPacketToAllClient(packet);

            return true;
        }
    }
}
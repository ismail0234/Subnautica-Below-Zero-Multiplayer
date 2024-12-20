namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class UpdatedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleUpdatedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.EntityId);
            if (entity == null)
            {
                return false;
            }

            if (entity.OwnershipId != profile.UniqueId)
            {
                return false;
            }

            entity.Position = packet.Position;
            entity.Rotation = packet.Rotation;
            packet.PlayerId = profile.PlayerId;

            if (entity.TechType != TechType.SpyPenguin && entity.TechType != TechType.MapRoomCamera)
            {
                profile.SetPosition(packet.Position, packet.Rotation);
            }

            profile.SendPacketToOtherClients(packet, true);
            return true;
        }
    }
}

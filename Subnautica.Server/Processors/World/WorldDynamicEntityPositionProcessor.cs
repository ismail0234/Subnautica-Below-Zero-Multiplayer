namespace Subnautica.Server.Processors.World
{
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldDynamicEntityPositionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldDynamicEntityPositionArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            foreach (var item in packet.Positions.ToList())
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(item.Id);
                if (entity == null || entity.IsUsingByPlayer || !entity.IsMine(profile.UniqueId) || entity.ParentId.IsNotNull())
                {
                    packet.Positions.Remove(item);
                    continue;
                }

                var distance = profile.Position.Distance(entity.Position);
                if (distance < API.Features.Network.DynamicEntity.VisibilityDistance)
                {
                    entity.Position = item.Position.ToZeroVector3();
                    entity.Rotation = item.Rotation.ToZeroQuaternion();
                }
                else
                {
                    packet.Positions.Remove(item);
                }
            }

            if (packet.Positions.Count > 0)
            {
                profile.SendPacketToOtherClients(packet, true);
            }

            return true;
        }
    }
}
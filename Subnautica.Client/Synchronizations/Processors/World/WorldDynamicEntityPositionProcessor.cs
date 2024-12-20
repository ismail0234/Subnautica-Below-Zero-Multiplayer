namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

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
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.WorldDynamicEntityPositionArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var item in packet.Positions)
            {
                var entity = Network.DynamicEntity.GetEntity(item.Id);
                if (entity != null)
                {
                    entity.Position = item.Position.ToZeroVector3();
                    entity.Rotation = item.Rotation.ToZeroQuaternion();
                }
            }

            return true;
        }
    }
}

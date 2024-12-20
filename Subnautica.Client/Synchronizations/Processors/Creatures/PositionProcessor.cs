namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class PositionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldCreaturePositionArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var item in packet.Positions)
            {
                Network.Creatures.SwimTo(item.CreatureId, item.Position.ToVector3(), item.Rotation.ToQuaternion());
            }

            return true;
        }
    }
}

namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldEntityActionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldEntityActionArgs>();
            if (packet == null)
            {
                return false;
            }

            return WorldEntityProcessor.ExecuteProcessor(packet.Entity, packet.GetPacketOwnerId(), false);
        }
    }
}
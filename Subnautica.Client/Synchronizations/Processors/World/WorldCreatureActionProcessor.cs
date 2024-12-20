namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldCreatureActionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureProcessArgs>();
            if (packet == null)
            {
                return false;
            }

            return WorldCreatureProcessor.ExecuteProcessor(packet.Component, packet.GetPacketOwnerId(), packet.ProcessTime, packet.CreatureType, packet.CreatureId);
        }
    }
}

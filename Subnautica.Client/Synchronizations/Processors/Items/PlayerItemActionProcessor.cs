namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class PlayerItemActionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerItemActionArgs>();
            if (packet == null)
            {
                return false;
            }

            return PlayerItemProcessor.ExecuteProcessor(packet.Item, packet.GetPacketOwnerId());
        }
    }
}
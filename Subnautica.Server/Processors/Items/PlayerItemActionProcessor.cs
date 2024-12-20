namespace Subnautica.Server.Processors.Items
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

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
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.PlayerItemActionArgs>();
            if (packet == null || packet.Item == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            return PlayerItemProcessor.ExecuteProcessor(profile, packet);
        }
    }
}

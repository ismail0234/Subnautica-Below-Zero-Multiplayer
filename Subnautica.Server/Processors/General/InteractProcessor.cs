namespace Subnautica.Server.Processors.General
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class InteractProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.InteractArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!packet.IsOpening)
            {
                if (Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId))
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);
                }
            }

            return true;
        }
    }
}

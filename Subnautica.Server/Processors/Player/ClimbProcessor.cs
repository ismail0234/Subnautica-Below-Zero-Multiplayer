namespace Subnautica.Server.Processors.Player
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ClimbProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerClimbArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var climbUniqueId = API.Features.Network.Identifier.GetClimbUniqueId(packet.UniqueId);
            if (climbUniqueId.IsNull())
            {
                return false;
            }

            if (Server.Instance.Logices.Interact.IsBlocked(climbUniqueId))
            {
                return false;
            }

            Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, climbUniqueId, true);

            if (packet.Duration <= 0f)
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.Climb + 0.1f);
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, packet.Duration + 0.1f);
            }

            profile.SendPacketToAllClient(packet);
            return true;
        }
    }
}

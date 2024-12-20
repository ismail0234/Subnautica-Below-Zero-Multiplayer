namespace Subnautica.Server.Processors.Player
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class UseableDiveHatchProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.UseableDiveHatchArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
            {
                return false;
            }

            Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);

            if (packet.IsLifePod)
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.LifePodUseableDiveHatch);
            }
            else if (packet.IsBulkHead)
            {
                if (packet.IsMoonpoolExpansion)
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.BulkHeadUseableDiveHatch + 0.25f);
                }
                else
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.BulkHeadUseableDiveHatch);
                }
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.NormalUseableDiveHatch);
            }

            profile.SendPacketToAllClient(packet);
            return true;
        }
    }
}
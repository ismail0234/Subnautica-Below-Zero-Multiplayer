namespace Subnautica.Server.Processors.Building
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class DeconstructionBeginProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.DeconstructionBeginArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
            {
                return false;
            }

            if (Server.Instance.Storages.Construction.IsDeconstructable(packet.UniqueId))
            {
                if (Server.Instance.Storages.Construction.UpdateConstructionAmount(packet.UniqueId, 0.98f))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.ConstructionTimeout);

                    packet.Id = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId).Id;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                packet.IsFailed = true;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}
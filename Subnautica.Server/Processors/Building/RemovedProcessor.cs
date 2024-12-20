namespace Subnautica.Server.Processors.Building
{
    using Server.Core;
    
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class RemovedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionRemovedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Storages.Construction.ConstructionRemove(packet.UniqueId, packet.Cell))
            {
                profile.SendPacketToAllClient(packet);
            }

            Server.Instance.Storages.PictureFrame.RemoveImage(packet.UniqueId);
            return true;
        }
    }
}

namespace Subnautica.Server.Processors.Building
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class MetadataRequestProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.MetadataComponentArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
            if (construction == null)
            {
                return this.SendErrorLog(string.Format("MetadataRequest, Construction Not Found. UniqueId: {0}, TechType: {1}, Component Type: {2}", packet.UniqueId, packet.TechType, packet.Component == null ? "NULL" : packet.Component.GetType().ToString()));
            }

            packet.TechType = construction.TechType;

            MetadataProcessor.ExecuteProcessor(profile, packet, construction);
            return true;
        }
    }
}
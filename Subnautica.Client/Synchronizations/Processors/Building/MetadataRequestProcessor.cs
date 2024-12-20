namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Models.Core;

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
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.MetadataComponentArgs>();
            if (packet.UniqueId.IsNull())
            {
                Log.Error(string.Format("Metadata Processor Unique Is Empty. TechType: {0}", packet.TechType));
                return false;
            }

            if (packet.TechType == TechType.None)
            {
                Log.Error(string.Format("Metadata Processor TechType Is None. UniqueId: {0}", packet.UniqueId));
                return false;
            }

            MetadataProcessor.ExecuteProcessor(packet.TechType, packet.UniqueId, packet);
            return true;
        }
    }
}

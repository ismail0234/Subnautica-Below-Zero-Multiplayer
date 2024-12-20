namespace Subnautica.Server.Processors.General
{
    using System.Collections.Generic;
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ResourceDiscoverProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ResourceDiscoverArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Storages.World.AddDiscoveredResource(packet.TechType))
            {
                packet.MapRooms = this.GetMapRooms();

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }

        /**
         *
         * Harita odalarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<string> GetMapRooms()
        {
            var response = new List<string>();

            foreach (var item in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMapRoom))
            {
                response.Add(item.Value.UniqueId);
            }

            return response;
        }
    }
}
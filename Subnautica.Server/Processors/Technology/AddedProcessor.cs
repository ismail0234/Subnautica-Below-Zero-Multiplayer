namespace Subnautica.Server.Processors.Technology
{
    using Server.Core;
    
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Technology;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class AddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.TechnologyAddedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            TechnologyItem item = new TechnologyItem()
            {
                TechType      = packet.TechType,
                Unlocked      = 1,
                TotalFragment = 1
            };

            if (Server.Instance.Storages.Technology.AddTechnology(item))
            {
                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}

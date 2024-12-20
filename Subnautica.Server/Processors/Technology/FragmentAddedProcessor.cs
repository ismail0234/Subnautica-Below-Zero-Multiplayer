namespace Subnautica.Server.Processors.Technology
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FragmentAddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.TechnologyFragmentAddedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var technology = Server.Instance.Storages.Technology.GetTechnology(packet.TechType, packet.TotalFragment);
            technology.Unlocked++;

            if (packet.UniqueId.IsNotNull())
            {
                technology.Fragments.Add(packet.UniqueId);
            }

            if (Server.Instance.Storages.Technology.AddTechnology(technology))
            {
                packet.Unlocked = technology.Unlocked;

                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}

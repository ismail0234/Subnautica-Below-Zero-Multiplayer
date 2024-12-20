namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class RadioTowerProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryRadioTowerArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsTOMUsing)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                {
                    return false;
                }

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.RadioTowerTomUsing);

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

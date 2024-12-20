namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ShieldBaseProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryShieldBaseArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            packet.Time = Server.Instance.Logices.World.GetServerTime() + 0.25f;

            if (packet.IsEntered)
            {
                if (Server.Instance.Storages.Story.Storage.ShieldBase.Enter())
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

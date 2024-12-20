namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BridgeProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryBridgeArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            packet.Time += 0.1f;

            if (packet.IsClickedFluid)
            {
                if (Server.Instance.Storages.Story.CompleteGoal(packet.StoryKey))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.IsClickedExtend)
            {
                if (Server.Instance.Storages.Story.Storage.Bridge.Extend(Server.Instance.Logices.World.GetServerTime(), packet.Time))
                {
                    packet.Time             = Server.Instance.Storages.Story.Storage.Bridge.Time;
                    packet.IsFirstExtension = Server.Instance.Storages.Story.Storage.Bridge.IsFirstExtension;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.IsClickedRetract)
            {
                if (Server.Instance.Storages.Story.Storage.Bridge.Retract(Server.Instance.Logices.World.GetServerTime(), packet.Time))
                {
                    packet.Time             = Server.Instance.Storages.Story.Storage.Bridge.Time;
                    packet.IsFirstExtension = Server.Instance.Storages.Story.Storage.Bridge.IsFirstExtension;

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

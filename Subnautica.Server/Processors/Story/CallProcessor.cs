namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CallProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryCallArgs>();
            if (packet == null || string.IsNullOrEmpty(packet.GoalKey.Trim()))
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Storages.Story.CompleteCall(packet.GoalKey) && Server.Instance.Storages.Story.CompleteGoal(packet.TargetGoalKey, global::Story.GoalType.PDA, false))
            {
                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

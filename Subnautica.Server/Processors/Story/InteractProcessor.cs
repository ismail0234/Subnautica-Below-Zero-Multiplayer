namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class InteractProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryInteractArgs>();
            if (packet == null || string.IsNullOrEmpty(packet.UniqueId))
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(packet.CinematicType.ToString()))
            {
                if (Server.Instance.Storages.Story.CompleteGoal(packet.GoalKey, global::Story.GoalType.Story, false))
                {
                    Server.Instance.Logices.StoryTrigger.CompleteTrigger(packet.CinematicType.ToString());

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}
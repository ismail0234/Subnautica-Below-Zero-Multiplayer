namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class TriggerProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryTriggerArgs>();
            if (packet == null || string.IsNullOrEmpty(packet.GoalKey.Trim()))
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.GoalType == global::Story.GoalType.Call && !Server.Instance.Storages.Story.IsGoalComplete(packet.GoalKey))
            {
                Server.Instance.Storages.Story.SetIncomingCall(packet.GoalKey);
            }

            if (packet.CinematicType != StoryCinematicType.None)
            {
                Log.Info("[DEBUG] TriggerProcessor -> UniqueId: " + packet.GoalKey + ", Goal: " + packet.CinematicType.ToString() + ", IsCompleteable: " + Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(packet.CinematicType.ToString()));
                if (!Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(packet.CinematicType.ToString()))
                {
                    return false;
                }

                Server.Instance.Logices.StoryTrigger.CompleteTrigger(packet.CinematicType.ToString());
            }

            if (Server.Instance.Logices.StoryTrigger.IsTriggerExists(packet.GoalKey))
            {
                Server.Instance.Logices.StoryTrigger.AddCheckTrigger(packet.GoalKey);
            }
            else
            {
                if (this.IsSpecialGoal(packet.GoalKey))
                {
                    if (profile.CompleteGoal(packet.GoalKey, packet.GoalType, packet.IsPlayMuted))
                    {
                        profile.SendPacket(packet);
                    }
                }
                else
                {
                    if (Server.Instance.Storages.Story.CompleteGoal(packet.GoalKey, packet.GoalType, packet.IsPlayMuted))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Özel hedef olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsSpecialGoal(string goalKey)
        {
            return goalKey.Contains("EnteringVoid") || goalKey.Contains("Log_ExplorationHint");
        }
    }
}

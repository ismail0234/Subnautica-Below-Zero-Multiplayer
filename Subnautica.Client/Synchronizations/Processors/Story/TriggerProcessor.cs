namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

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
        private List<ServerModel.StoryTriggerArgs> GoalsQueue = new List<ServerModel.StoryTriggerArgs>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.StoryTriggerArgs>();
            if (packet.IsTrigger)
            {
                if (packet.PlayerCount > 0)
                {
                    Network.Story.ShowWaitingPlayerMessage(packet.PlayerCount, packet.MaxPlayerCount);
                }
                else
                {
                    Network.Story.HideWaitingPlayerMessage();

                    if (packet.GoalKey.IsNotNull())
                    {
                        var trigger = Network.Story.GetTriggerItem(packet.GoalKey);
                        if (trigger != null && trigger.IsCustomDoor)
                        {
                            UWE.CoroutineHost.StartCoroutine(this.OpenCustomDoorAsync(trigger.GoalKey));
                        }
                        else 
                        {
                            this.GoalsQueue.Add(packet);
                        }
                    }
                }
            }
            else
            {
                Log.Info("TRIGGER GOAL - 1: " + packet.GoalKey + ", type: " + packet.GoalType + ", muted: " + packet.IsPlayMuted + ", T1: " + Network.Session.GetWorldTime() + ", T2: " + packet.TriggerTime);

                this.GoalsQueue.Add(packet);
            }

            return true;
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            if (this.GoalsQueue.Count > 0)
            {
                foreach (var packet in this.GoalsQueue.ToList())
                {
                    if (DayNightCycle.main.timePassedAsFloat < packet.TriggerTime)
                    {
                        continue;
                    }

                    if (PlayerCinematicController.cinematicModeCount > 0 && packet.GoalType == global::Story.GoalType.PDA)
                    {
                        continue;
                    }

                    this.GoalsQueue.Remove(packet);

                    if (packet.IsPlayMuted)
                    {
                        Log.Info("OnFixedUpdate - 1: " + packet.GoalKey + ", GoalType: " + packet.GoalType + ", IsPlayMuted: " + packet.IsPlayMuted + ", IsStoryGoalMuted: " + packet.IsStoryGoalMuted);

                        Network.Story.MuteFutureStoryGoal(packet.GoalKey);
                    }
                    else
                    {
                        Log.Info("OnFixedUpdate - 2: " + packet.GoalKey + ", GoalType: " + packet.GoalType + ", IsPlayMuted: " + packet.IsPlayMuted + ", IsStoryGoalMuted: " + packet.IsStoryGoalMuted);

                        Network.Story.GoalExecute(packet.GoalKey, packet.GoalType, packet.IsStoryGoalMuted);
                    }
                }
            }
        }

        /**
         *
         * Kapıyı açar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator OpenCustomDoorAsync(string uniqueId)
        {
            var doorway = Network.Identifier.GetComponentByGameObject<PrecursorDoorway>(uniqueId);
            if (doorway)
            {
                doorway.DisableField();

                yield return new WaitForSecondsRealtime(doorway.colorControl.duration + 0.1f);

                if (doorway)
                {
                    World.DestroyGameObject(doorway.gameObject);
                }
            }
        }

        /**
         *
         * Hedef tetiklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryGoalTriggering(StoryGoalTriggeringEventArgs ev)
        {
            if (!IsEndGameTrigger(ev.StoryKey))
            {
                ev.IsAllowed = false;

                if (IsTriggerable(ev.StoryKey))
                {
                    ServerModel.StoryTriggerArgs result = new ServerModel.StoryTriggerArgs()
                    {
                        GoalKey          = ev.StoryKey,
                        GoalType         = ev.GoalType,
                        CinematicType    = ev.CinematicType,
                        IsPlayMuted      = ev.IsPlayMuted,
                        IsStoryGoalMuted = ev.IsStoryGoalMuted,
                    };

                    NetworkClient.SendPacket(result);
                }
            }
        }

        /**
         *
         * Tetiklenebilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsTriggerable(string goalKey)
        {
            if (goalKey.Contains("PlayerDiving"))
            {
                return false;
            }

            if (goalKey.Contains("EnteringVoid") && Network.Session.GetWorldTime() < 500)
            {
                return false;
            }

            return true;
        }

        /**
         *
         * Oyun sonu tetikleyicileri mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsEndGameTrigger(string goalKey)
        {
            if (goalKey.Contains("EnteringVoid") && Network.Session.GetWorldTime() >= 500)
            {
                return true;
            }

            return goalKey.Contains("EndGame5GateOpen") || goalKey.Contains("EndGame5TeleportBegin") || goalKey.Contains("EndGame5TeleportEnd") || goalKey.Contains("EndGame5Homeworld") || goalKey.Contains("ShowGameCredits");
        }
    }
}
namespace Subnautica.Client.Synchronizations.InitialSync
{
    using Story;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Synchronizations.Processors.Story;
    using Subnautica.Network.Models.Storage.Story.StoryGoals;

    public class StoryProcessor
    {
        /**
         *
         * Hedefleri tamamlandı yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGoalsCompleteInitialized()
        {
            using (EventBlocker.Create(ProcessType.NotificationAdded))
            {
                foreach (var goal in Network.Session.Current.Story.CompletedGoals)
                {
                    StoryProcessor.ProcessCompletedGoal(goal);
                }

                foreach (var goal in Network.Session.Current.PlayerSpecialGoals)
                {
                    StoryProcessor.ProcessCompletedGoal(goal);
                }
            }

            using (EventBlocker.Create(ProcessType.DeconstructionBegin))
            {
                StoryGoalManager.main.compoundGoalTracker.Initialize(StoryGoalManager.main.completedGoals);
                StoryGoalManager.main.branchingGoalTracker.Initialize(StoryGoalManager.main.completedGoals);
                StoryGoalManager.main.onGoalUnlockTracker.Initialize(StoryGoalManager.main.completedGoals);
                StoryGoalManager.main.initialized = true;
            }

            foreach (var signal in Network.Session.Current.Story.Signals)
            {
                if (!signal.IsRemoved)
                {
                    SignalProcessor.SpawnSignal(signal);
                }
            }

            uGUI_PopupNotification.main.IncomingCall(Network.Session.Current.Story.IncomingCallGoalKey);
        }        
        
        /**
         *
         * Hikaye ve PDA loglarını işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void ProcessCompletedGoal(ZeroStoryGoal goal)
        {
            if (goal.IsPlayMuted)
            {
                StoryGoalManager.main.mutedStoryGoals.Add(goal.Key);
            }
            else
            {
                StoryGoalManager.main.completedGoals.Add(goal.Key);
            }

            if (goal.GoalType == GoalType.PDA)
            {
                PDALog.time = goal.FinishedTime;
                PDALog.Add(goal.Key, false);
                PDALog.time = DayNightCycle.main.timePassedAsFloat;
            }
            else if (goal.GoalType == GoalType.Encyclopedia)
            {
                if (PDAEncyclopedia.GetEntryData(goal.Key, out var entryData))
                {
                    PDAEncyclopedia.Add(goal.Key, false, false);

                    if (entryData.hidden)
                    {
                        PDAEncyclopedia.Reveal(goal.Key, false);
                    }
                }
            }
        }
    }
}

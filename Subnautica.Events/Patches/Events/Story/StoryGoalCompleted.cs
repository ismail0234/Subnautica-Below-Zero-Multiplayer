namespace Subnautica.Events.Patches.Events.Story
{
    using global::Story;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Story.StoryGoal), nameof(global::Story.StoryGoal.Trigger))]
    public static class StoryGoalCompleted
    {
        private static bool Prefix(global::Story.StoryGoal __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }
            
            if (string.IsNullOrEmpty(__instance.key) || !__instance.playMuted || (!GameModeManager.GetOption<bool>(GameOption.Story) && !__instance.playInCreative))
            {
                return true;
            }

            try
            {
                StoryGoalTriggeringEventArgs args = new StoryGoalTriggeringEventArgs(__instance.key, __instance.goalType, __instance.playMuted);

                Handlers.Story.OnStoryGoalTriggering(args);
            }
            catch (Exception e)
            {
                Log.Error($"StoryGoalCompleted.Prefix_1: {e}\n{e.StackTrace}");
            }

            if (__instance.delay == 0.0 && __instance.goalType == GoalType.Story && !__instance.checkPlayerSafety && !__instance.checkInBase && __instance.playInCinematics)
            {
                return true;
            }

            if (__instance.goalType != GoalType.Encyclopedia || !PDAEncyclopedia.GetEntryData(__instance.key, out var entryData) || entryData.kind != PDAEncyclopedia.EntryData.Kind.Journal)
            {
                return true;
            }

            var logKey = string.Format("Log_{0}", __instance.key);
            if (!PDALog.GetEntryData(logKey, out PDALog.EntryData _) || StoryGoalManager.main.IsGoalComplete(logKey))
            {
                return true;
            }

            try
            {
                StoryGoalTriggeringEventArgs args = new StoryGoalTriggeringEventArgs(__instance.key, __instance.goalType, __instance.playMuted);

                Handlers.Story.OnStoryGoalTriggering(args);
            }
            catch (Exception e)
            {
                Log.Error($"StoryGoalCompleted.Prefix_2: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(global::Story.StoryGoal), nameof(global::Story.StoryGoal.Execute))]
    public static class StoryGoalCompletedExecute
    {
        private static bool Prefix(string key, GoalType goalType, bool playInCinematics, bool playInCreative)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!GameModeManager.GetOption<bool>(GameOption.Story) && !playInCreative)
            {
                return false;
            }

            try
            {
                StoryGoalTriggeringEventArgs args = new StoryGoalTriggeringEventArgs(key, goalType, false, global::Story.StoryGoalManager.main.IsStoryGoalMuted(key) || !playInCinematics && global::Player.main.cinematicModeActive);

                Handlers.Story.OnStoryGoalTriggering(args);

                if (!args.IsAllowed)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error($"StoryGoalCompletedExecute.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
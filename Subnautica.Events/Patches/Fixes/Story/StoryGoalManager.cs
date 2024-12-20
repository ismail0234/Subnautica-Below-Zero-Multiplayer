namespace Subnautica.Events.Patches.Fixes.Story
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class StoryGoalManager
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Story.StoryGoalManager), nameof(global::Story.StoryGoalManager.MuteFutureStoryGoal))]
        private static bool MuteFutureStoryGoal()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Story.StoryGoalManager), nameof(global::Story.StoryGoalManager.OnSceneObjectsLoaded))]
        private static bool OnSceneObjectsLoaded(global::Story.StoryGoalManager __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Story.StoryGoalManager), nameof(global::Story.StoryGoalManager.OnGoalComplete))]
        private static void OnGoalComplete(global::Story.StoryGoalManager __instance, string key)
        {
            if (Network.IsMultiplayerActive)
            {
                if (string.Compare(key, __instance.precursorArtifactScanGoal, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    __instance.precursorScanCount = 0;

                    var isCompleted = PDAScanner.complete.Contains(TechType.PrecursorTechGeneric);
                    var unlockCount = PDAScanner.GetPartialEntryByKey(TechType.PrecursorTechGeneric, out var entry) ? entry.unlocked : 0;

                    foreach (var precursorScanBounty in global::Player.main.pdaData.precursorScanBounties)
                    {
                        if (!__instance.IsGoalComplete(precursorScanBounty.storyGoal) && (isCompleted || unlockCount >= precursorScanBounty.threshold))
                        {
                            __instance.OnGoalComplete(precursorScanBounty.storyGoal);
                        }
                    }
                }
            }
        }
    }
}
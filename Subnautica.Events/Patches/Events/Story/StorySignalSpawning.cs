namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Story.UnlockSignalData), nameof(global::Story.UnlockSignalData.Trigger))]
    public static class StorySignalSpawning
    {
        private static bool Prefix(global::Story.UnlockSignalData __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                StorySignalSpawningEventArgs args = new StorySignalSpawningEventArgs(__instance.signalType, __instance.targetPosition, __instance.targetDescription);

                Handlers.Story.OnStorySignalSpawning(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"StorySignalSpawning.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
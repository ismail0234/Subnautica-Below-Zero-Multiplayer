namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class StoryCinematicToggle
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.StartCinematicMode))]
        private static void StartCinematicMode(global::PlayerCinematicController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    StoryCinematicStartedEventArgs args = new StoryCinematicStartedEventArgs(__instance.name);

                    Handlers.Story.OnStoryCinematicStarted(args);
                }
                catch (Exception e)
                {
                    Log.Error($"StoryCinematicToggle.StartCinematicMode: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.OnPlayerCinematicModeEnd))]
        private static void OnPlayerCinematicModeEnd(global::PlayerCinematicController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    StoryCinematicCompletedEventArgs args = new StoryCinematicCompletedEventArgs(__instance.name);

                    Handlers.Story.OnStoryCinematicCompleted(args);
                }
                catch (Exception e)
                {
                    Log.Error($"StoryCinematicToggle.OnPlayerCinematicModeEnd: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

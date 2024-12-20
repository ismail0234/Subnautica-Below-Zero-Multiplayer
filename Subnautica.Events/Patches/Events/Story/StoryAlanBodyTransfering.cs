namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PrecursorCharacter), nameof(global::PrecursorCharacter.OnHandClick))]
    public static class StoryAlanBodyTransfering
    {
        private static bool Prefix(global::PrecursorCharacter __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.disableInteractions || __instance.availableInteractions.Count == 0 || PDAScanner.IsScanning())
            {
                return false;
            }

            var cinematicType = CinematicTriggering.GetCinematicType(__instance.gameObject);
            if (cinematicType == StoryCinematicType.None)
            {
                return true;
            }

            var lwe = __instance.gameObject.GetComponentInParent<LargeWorldEntity>();
            if (lwe == null)
            {
                return true;
            }

            var story = __instance.availableInteractions[0].triggerOnClick;
            if (story == null)
            {
                return false;
            }

            try
            {
                StoryGoalTriggeringEventArgs args = new StoryGoalTriggeringEventArgs(story.key, story.goalType, false, false, cinematicType);

                Handlers.Story.OnStoryGoalTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"StoryAlanBodyTransfering.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
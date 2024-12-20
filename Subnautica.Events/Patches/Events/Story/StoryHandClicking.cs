namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::StoryHandTarget), nameof(global::StoryHandTarget.OnHandClick))]
    public static class StoryHandClicking
    {
        private static bool Prefix(global::StoryHandTarget __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.primaryTooltip.Contains("PDA"))
            {
                return true;
            }

            if (__instance.goal.IsComplete())
            {
                return false;
            }

            var lwe = __instance.gameObject.GetComponentInParent<LargeWorldEntity>();
            if (lwe == null)
            {
                return true;
            }

            var cinematicType = CinematicTriggering.GetCinematicType(__instance.gameObject);
            if (cinematicType == StoryCinematicType.None)
            {
                return true;
            }

            try
            {
                StoryHandClickingEventArgs args = new StoryHandClickingEventArgs(Network.Identifier.GetIdentityId(lwe.gameObject, false), __instance.goal.key, cinematicType);

                Handlers.Story.OnStoryHandClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"StoryHandClicking.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
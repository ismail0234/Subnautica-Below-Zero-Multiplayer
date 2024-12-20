namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.OnDockingTimelineCompleted))]
    public static class BaseMoonpoolExpansionDockingTimelineCompleting
    {
        private static bool Prefix(global::MoonpoolExpansionManager __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.IsOccupied() || __instance.isFullyDocked)
            {
                return false;
            }

            try
            {
                BaseMoonpoolExpansionDockingTimelineCompletingEventArgs args = new BaseMoonpoolExpansionDockingTimelineCompletingEventArgs(__instance.gameObject);

                Handlers.Furnitures.OnBaseMoonpoolExpansionDockingTimelineCompleting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMoonpoolExpansionDockingTimelineCompleting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
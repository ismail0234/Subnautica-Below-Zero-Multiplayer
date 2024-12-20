namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.OnUndockingTimelineCompleted))]
    public static class BaseMoonpoolExpansionUndockingTimelineCompleting
    {
        private static bool Prefix(global::MoonpoolExpansionManager __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs args = new BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs(__instance.gameObject);

                Handlers.Furnitures.OnBaseMoonpoolExpansionUndockingTimelineCompleting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMoonpoolExpansionUndockingTimelineCompleting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
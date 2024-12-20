namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch]
    public static class BaseMoonpoolExpansionTail
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.DockTail))]
        private static bool DockTail(global::MoonpoolExpansionManager __instance, global::SeaTruckSegment newTail)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                BaseMoonpoolExpansionDockTailEventArgs args = new BaseMoonpoolExpansionDockTailEventArgs(__instance.gameObject, newTail);

                Handlers.Furnitures.OnBaseMoonpoolExpansionDockTail(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMoonpoolExpansionTail.DockTail: {e}\n{e.StackTrace}");
                return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.UndockTail))]
        private static bool UndockTail(global::MoonpoolExpansionManager __instance, bool withEjection)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                BaseMoonpoolExpansionUndockTailEventArgs args = new BaseMoonpoolExpansionUndockTailEventArgs(__instance.gameObject, withEjection);

                Handlers.Furnitures.OnBaseMoonpoolExpansionUndockTail(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseMoonpoolExpansionTail.UndockTail: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class SupplyDropManager
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SupplyDropManager), nameof(global::SupplyDropManager.NotifyGoalComplete))]
        private static bool NotifyGoalComplete()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SupplyDropManager), nameof(global::SupplyDropManager.ForceDrop))]
        private static bool ForceDrop()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

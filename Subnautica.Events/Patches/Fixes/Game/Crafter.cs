namespace Subnautica.Events.Patches.Fixes.Game
{
    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch]
    public static class Crafter
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.OnCraftingBegin))]
        private static void GhostCrafter_OnCraftingBegin(global::GhostCrafter __instance, ref float duration)
        {
            if (Network.IsMultiplayerActive)
            {
                if (duration > 20f)
                {
                    duration = 20f;
                }
            }
        }
    }
}

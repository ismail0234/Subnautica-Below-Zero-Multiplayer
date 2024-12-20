namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class DeployableStorage
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ColoredLabel), nameof(global::ColoredLabel.OnProtoDeserialize))]
        private static bool ColoredLabelUpdate(global::ColoredLabel __instance)
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

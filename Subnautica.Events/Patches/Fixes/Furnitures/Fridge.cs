namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Fridge
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Fridge), nameof(global::Fridge.OnUpdate))]
        private static bool OnUpdate()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Fridge), nameof(global::Fridge.AddItem))]
        private static bool AddItem()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Fridge), nameof(global::Fridge.RemoveItem))]
        private static bool RemoveItem()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Eatable), nameof(global::Eatable.StartDespawnInvoke))]
        private static bool StartDespawnInvoke()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}
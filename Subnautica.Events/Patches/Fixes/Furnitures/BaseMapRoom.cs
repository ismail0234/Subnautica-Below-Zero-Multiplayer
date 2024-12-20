namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class BaseMapRoom
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomFunctionality), nameof(global::MapRoomFunctionality.UpdateBlips))]
        private static bool UpdateBlips()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomFunctionality), nameof(global::MapRoomFunctionality.UpdateCameraBlips))]
        private static bool UpdateCameraBlips()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomFunctionality), nameof(global::MapRoomFunctionality.ObtainResourceNodes))]
        private static bool ObtainResourceNodes()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_ResourceTracker), nameof(global::uGUI_ResourceTracker.GatherScanned))]
        private static bool GatherScanned()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.OnResourceRemoved))]
        private static bool OnResourceRemoved()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_MapRoomScanner), nameof(global::uGUI_MapRoomScanner.OnScanRangeChanged))]
        private static bool OnScanRangeChanged()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

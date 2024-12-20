namespace Subnautica.Events.Patches.SerializeOptimizers
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class SeaTruckDockingBay
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.OnProtoSerialize))]
        private static bool SeaTruckDockingBay_OnProtoSerialize()
        {
            return !Network.IsMultiplayerActive;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.OnProtoDeserialize))]
        private static bool SeaTruckDockingBay_OnProtoDeserialize(global::SeaTruckDockingBay __instance)
        {
            return !Network.IsMultiplayerActive;
        }
    }
}
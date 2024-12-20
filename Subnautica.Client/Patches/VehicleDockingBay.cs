namespace Subnautica.Client.Patches
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.World;

    [HarmonyPatch]
    internal class VehicleDockingBay
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.Start))]
        private static void Start(global::VehicleDockingBay __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
            }
        }
    }
}

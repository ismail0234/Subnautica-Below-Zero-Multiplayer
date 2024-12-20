namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    [HarmonyPatch]
    public class BaseMoonpoolExpansion
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.UpdateArmsAnim))]
        private static bool OnPlayerdCinematicMxodexEx1nd(global::MoonpoolExpansionManager __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return API.Features.World.IsLoaded;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::MoonpoolExpansionManager), nameof(global::MoonpoolExpansionManager.StartDocking))]
        private static void OnPlayerdCixnematicxzMxodx1xexxEx1xxnd(global::MoonpoolExpansionManager __instance)
        {
            if (Network.IsMultiplayerActive && __instance.IsOccupied() && !__instance.isFullyDocked && !__instance.isLoading && !__instance.dockedHead.IsPiloted())
            {
                global::Player.main.UnfreezeStats();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MoonpoolExpansionManager), nameof(MoonpoolExpansionManager.FixedUpdate))]
        private static void MoonpoolExpansionManager_FixedUpdate(MoonpoolExpansionManager __instance)
        {
            if (Network.IsMultiplayerActive && __instance.Terminal.dockingBay && __instance.Terminal.dockingBay.GetDockedObject())
            {
                if (ZeroVector3.Distance(__instance.Terminal.transform.position, global::Player.main.transform.position) <= 6f)
                {
                    __instance.Terminal.UpdateCameras();
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MoonpoolExpansionCamera), nameof(MoonpoolExpansionCamera.Awake))]
        private static void MoonpoolExpansionCamera_Awake(MoonpoolExpansionCamera __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.controlledCamera.cullingMask |= 1 << LayerID.Player;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.RepairVehicle))]
        private static bool OnPlayerdCixnematicMxodexxEx1nd(global::VehicleDockingBay __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Dockable), nameof(global::Dockable.OnProtoSerialize))]
        private static bool OnPlayerdCixnematicxzMxodexxEx1xnd(global::Dockable __instance)
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

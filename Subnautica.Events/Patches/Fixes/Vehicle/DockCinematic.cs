namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    [HarmonyPatch]
    public class DockCinematic
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.StartCinematicMode))]
        private static void StartCinematicMode(global::PlayerCinematicController __instance)
        {
            if (__instance.player == null && __instance.name.Contains("DockPlayerCinematic"))
            {
                DockCinematic.ToggleRequestCrosshair(false);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.BreakCinematic))]
        private static void BreakCinematic(global::PlayerCinematicController __instance)
        {
            if (__instance.player == null && __instance.cinematicModeActive && __instance.name.Contains("DockPlayerCinematic"))
            {
                DockCinematic.ToggleRequestCrosshair(true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerCinematicController), nameof(global::PlayerCinematicController.EndCinematicMode))]
        private static void EndCinematicMode(global::PlayerCinematicController __instance)
        {
            if (__instance.player == null && __instance.cinematicModeActive && __instance.name.Contains("DockPlayerCinematic"))
            {
                DockCinematic.ToggleRequestCrosshair(true);
            }
        }

        /**
         *
         * Crosshair isteğini otomatikleştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void ToggleRequestCrosshair(bool isHide)
        {
            if (isHide)
            {
                PlayerCinematicController.cinematicModeCount++;
                HandReticle.main.RequestCrosshairHide();
            }
            else
            {
                PlayerCinematicController.cinematicModeCount--;
                HandReticle.main.UnrequestCrosshairHide();
            }
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class Ladder
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTrigger), nameof(global::CinematicModeTrigger.OnHandHover))]
        private static bool CinematicModeTrigger_OnHandHover(global::CinematicModeTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.showIconOnHandHover || !__instance.isValidHandTarget || PlayerCinematicController.cinematicModeCount > 0 || string.IsNullOrEmpty(__instance.handText))
            {
                return false;
            }

            if (!Events.Items.Climbing.ClimbTexts.Contains(__instance.handText))
            {
                return true;
            }

            var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(Network.Identifier.GetClimbUniqueId(uniqueId)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

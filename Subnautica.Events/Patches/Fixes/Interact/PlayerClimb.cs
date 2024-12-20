namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(CinematicModeTrigger), nameof(CinematicModeTrigger.OnHandHover))]
    public class PlayerClimb
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::CinematicModeTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.showIconOnHandHover || !__instance.isValidHandTarget || PlayerCinematicController.cinematicModeCount > 0 || string.IsNullOrEmpty(__instance.handText))
            {
                return false;
            }
                
            var component = __instance.gameObject.GetComponentInParent<global::Constructor>();
            if (component)
            {
                if (Network.HandTarget.IsBlocked(Network.Identifier.GetIdentityId(component.gameObject)))
                {
                    Interact.ShowUseDenyMessage();
                    return false;
                }
            }

            return true;
        }
    }
}

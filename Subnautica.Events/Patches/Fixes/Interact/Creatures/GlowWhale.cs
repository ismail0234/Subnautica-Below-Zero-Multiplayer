namespace Subnautica.Events.Patches.Fixes.Interact.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch]
    public class GlowWhale
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GlowWhaleRide), nameof(global::GlowWhaleRide.OnHandHover))]
        private static bool GlowWhaleRide_OnHandHover(global::CinematicModeTriggerBase __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var uniqueId = __instance.GetComponentInParent<global::GlowWhale>().gameObject.GetIdentityId();
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (Network.HandTarget.IsBlocked(uniqueId))
            {
                Subnautica.API.Features.Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTrigger), nameof(global::CinematicModeTrigger.OnHandHover))]
        private static bool CinematicModeTriggerBase_OnHandHover(global::CinematicModeTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var glowWhale = __instance.gameObject.GetComponentInParent<global::GlowWhale>();
            if (glowWhale == null)
            {
                return true;
            }

            var uniqueId = glowWhale.gameObject.GetIdentityId();
            if (uniqueId.IsNull())
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(uniqueId))
            {
                Subnautica.API.Features.Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

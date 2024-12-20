namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::GlowWhaleRide), nameof(global::GlowWhaleRide.OnHandClick))]
    public class GlowWhaleRideStarting
    {
        private static bool Prefix(global::GlowWhaleRide __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (PlayerCinematicController.cinematicModeCount > 0 || __instance.enabled == false || hand.player == null)
            {
                return false;
            }

            try
            {
                GlowWhaleRideStartingEventArgs args = new GlowWhaleRideStartingEventArgs(__instance.GetComponentInParent<global::GlowWhale>().gameObject.GetIdentityId());

                Handlers.Creatures.OnGlowWhaleRideStarting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"GlowWhaleRideStarting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}

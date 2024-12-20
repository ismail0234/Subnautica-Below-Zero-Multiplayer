namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::GlowWhaleRide), nameof(global::GlowWhaleRide.StopRide))]
    public class GlowWhaleRideStoped
    {
        private static void Prefix(global::GlowWhaleRide __instance)
        {
            if (Network.IsMultiplayerActive && __instance.ridden)
            {
                try
                {
                    GlowWhaleRideStopedEventArgs args = new GlowWhaleRideStopedEventArgs(__instance.GetComponentInParent<global::GlowWhale>().gameObject.GetIdentityId());

                    Handlers.Creatures.OnGlowWhaleRideStoped(args);
                }
                catch (Exception e)
                {
                    Log.Error($"GlowWhaleRideStoped.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

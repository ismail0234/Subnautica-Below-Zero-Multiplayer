namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public class GlowWhaleEyeCinematicStarting
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnHandClick))]
        private static bool OnHandClick(global::CinematicModeTriggerBase __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.triggerType != CinematicModeTriggerBase.TriggerType.HandTarget)
            {
                return true;
            }

            var glowWhale = __instance.gameObject.GetComponentInParent<global::GlowWhale>();
            if (glowWhale == null)
            {
                return true;
            }

            try
            {
                GlowWhaleEyeCinematicStartingEventArgs args = new GlowWhaleEyeCinematicStartingEventArgs(glowWhale.gameObject.GetIdentityId());

                Handlers.Creatures.OnGlowWhaleEyeCinematicStarting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"GlowWhaleEyeCinematicStarting.OnHandClick: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}

namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Enums.Creatures;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public class GlowWhaleSFXTriggered
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GlowWhaleSFXManager), nameof(global::GlowWhaleSFXManager.OnWhaleBreach))]
        private static void OnWhaleBreach(global::GlowWhaleSFXManager __instance)
        {
            CallEvent(__instance, GlowWhaleSFXType.WhaleBreach);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GlowWhaleSFXManager), nameof(global::GlowWhaleSFXManager.OnWhaleDive))]
        private static void OnWhaleDive(global::GlowWhaleSFXManager __instance)
        {
            CallEvent(__instance, GlowWhaleSFXType.WhaleDive);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GlowWhaleSFXManager), nameof(global::GlowWhaleSFXManager.OnGulpAnimation))]
        private static void OnGulpAnimation(global::GlowWhaleSFXManager __instance)
        {
            CallEvent(__instance, GlowWhaleSFXType.GulpAnimation);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::GlowWhaleSFXManager), nameof(global::GlowWhaleSFXManager.OnBreathAnimation))]
        private static void OnBreathAnimation(global::GlowWhaleSFXManager __instance)
        {
            CallEvent(__instance, GlowWhaleSFXType.BreathAnimation);
        }

        /**
         *
         * Olayı tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void CallEvent(global::GlowWhaleSFXManager __instance, GlowWhaleSFXType sfxType)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.GlowWhale))
            {
                try
                {
                    GlowWhaleSFXTriggeredEventArgs args = new GlowWhaleSFXTriggeredEventArgs(__instance.GetComponentInParent<global::GlowWhale>().gameObject.GetIdentityId(), sfxType);

                    Handlers.Creatures.OnGlowWhaleSFXTriggered(args);
                }
                catch (Exception e)
                {
                    Log.Error($"GlowWhaleSFXTriggered.CallEvent: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

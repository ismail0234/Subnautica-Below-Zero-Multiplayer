namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    [HarmonyPatch]
    public class Crash
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Crash), nameof(global::Crash.AnimateInflate))]
        private static bool AnimateInflate(global::Crash __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Crash), nameof(global::Crash.Detonate))]
        private static bool Detonate(global::Crash __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Creature), nameof(global::Creature.OnDisable))]
        private static void OnDisable(global::Creature __instance)
        {
            if (Network.IsMultiplayerActive && __instance.gameObject.GetTechType() == TechType.Crash)
            {
                __instance.CancelInvoke("Inflate");
                __instance.CancelInvoke("AnimateInflate");
                __instance.CancelInvoke("Detonate");
                __instance.CancelInvoke("OnCalmDown");
            }
        }
    }
}

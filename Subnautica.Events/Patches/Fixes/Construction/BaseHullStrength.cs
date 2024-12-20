namespace Subnautica.Events.Patches.Fixes.Construction
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class BaseHullStrength
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseFloodSim), nameof(global::BaseFloodSim.AddLeaks))]
        private static bool BaseFloodSim_AddLeaks(global::BaseFloodSim __instance)
        {
            if (Network.IsMultiplayerActive && !Network.IsHost)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseFloodSim), nameof(global::BaseFloodSim.ApplyDraining))]
        private static bool BaseFloodSim_ApplyDraining(global::BaseFloodSim __instance)
        {
            if (Network.IsMultiplayerActive && !Network.IsHost)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseFloodSim), nameof(global::BaseFloodSim.Step))]
        private static bool BaseFloodSim_Step(global::BaseFloodSim __instance)
        {
            if (Network.IsMultiplayerActive && !Network.IsHost)
            {
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(global::Leakable), nameof(global::Leakable.UpdateLeakPoints))]
        private static bool Leakable_UpdateLeakPoints(global::Leakable __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                foreach (VFXSubLeakPoint leakingLeakPoint in __instance.leakingLeakPoints)
                {
                    leakingLeakPoint.UpdateEffects();
                }

                return false;
            }

            return true;
        }
    }
}
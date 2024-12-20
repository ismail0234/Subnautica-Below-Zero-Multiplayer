namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.Network.Structures;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public class CrushDamage
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CrushDamage), nameof(global::CrushDamage.UpdateDepthClassification))]
        private static bool UpdateDepthClassification(global::CrushDamage __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.gameObject.activeInHierarchy)
            {
                float a = __instance.crushDepth;
                __instance.crushDepth = __instance.kBaseCrushDepth + __instance.extraCrushDepth;

                if (ZeroVector3.Distance(__instance.transform.position, global::Player.main.transform.position) < 225f)
                {
                    if (!Mathf.Approximately(a, __instance.crushDepth) && !uGUI.isLoading)
                    {
                        ErrorMessage.AddMessage(global::Language.main.GetFormat("CrushDepthNow", __instance.crushDepth));
                    }
                }
            }

            return false;
        }
    }
}

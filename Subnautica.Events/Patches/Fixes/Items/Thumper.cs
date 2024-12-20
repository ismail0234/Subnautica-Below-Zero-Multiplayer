namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public static class Thumper
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Thumper), nameof(global::Thumper.Update))]
        private static bool ThumperUpdate(global::Thumper __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.deployed)
            {
                return false;
            }

            if (__instance.energyMixin == null || __instance.energyMixin.IsDepleted())
            {
                __instance.Deploy(false);
            }
            else
            {
                if (__instance.lerpVfx)
                {
                    __instance.vfxRadius = Mathf.Lerp(__instance.vfxRadius, __instance.thumperEffectRadius, Time.deltaTime * 0.25f);
                    __instance.radiusIndicator.transform.localScale = Vector3.one * (__instance.vfxRadius * 2.1f);
                }
                else
                {
                    __instance.radiusIndicator.transform.localScale = Vector3.one;
                }

                __instance.UpdateControllerLightbarToToolBarValue();
            }

            return false;
        }
    }
}

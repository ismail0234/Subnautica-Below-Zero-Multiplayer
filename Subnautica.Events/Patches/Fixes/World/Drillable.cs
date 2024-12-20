namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Drillable), nameof(global::Drillable.Start))]
    public class Drillable
    {
        private static bool Prefix(global::Drillable __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.renderers == null)
            {
                __instance.renderers = __instance.GetComponentsInChildren<MeshRenderer>();
            }

            if (__instance.health == null)
            {
                __instance.health = new float[__instance.renderers.Length];

                for (int index = 0; index < __instance.health.Length; ++index)
                {
                    __instance.health[index] = 200f;
                }
            }

            var dominantResourceType = __instance.GetDominantResourceType();
            if (__instance.primaryTooltip.IsNull())
            {
                __instance.primaryTooltip = dominantResourceType.AsString();
            }

            if (__instance.secondaryTooltip.IsNull())
            {
                __instance.secondaryTooltip = global::Language.main.GetFormat<string, string>("DrillResourceTooltipFormat", global::Language.main.Get(dominantResourceType), global::Language.main.Get(TooltipFactory.techTypeTooltipStrings.Get(dominantResourceType)));
            }
                
            __instance.modelRootOffset = __instance.modelRoot.transform.position - __instance.transform.position;
            return false;
        }
    }
}
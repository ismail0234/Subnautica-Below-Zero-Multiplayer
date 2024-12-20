namespace Subnautica.Events.Patches.Fixes.Game
{
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public static class VFXConstructing
    {
        /**
         *
         * Süreleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, float> CrafterTimes { get; set; } = new Dictionary<string, float>();

        /**
         *
         * StartConstruction Yaması
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VFXConstructing), nameof(global::VFXConstructing.StartConstruction))]
        private static void StartConstruction(global::VFXConstructing __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = GetUniqueId(__instance);
                if (!string.IsNullOrEmpty(uniqueId))
                {
                    __instance.timeToConstruct += __instance.delay;

                    CrafterTimes[uniqueId] = DayNightCycle.main.timePassedAsFloat;
                }
            }
        }

        /**
         *
         * UpdateConstruct Yaması
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VFXConstructing), nameof(global::VFXConstructing.UpdateConstruct))]
        private static bool UpdateConstruct(global::VFXConstructing __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var uniqueId = GetUniqueId(__instance);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            if (CrafterTimes.TryGetValue(uniqueId, out var startedTime))
            {
                var buildTime = (__instance.timeToConstruct - startedTime);
                if (buildTime <= 0)
                {
                    __instance.constructed = 1f;
                }
                else
                {
                    __instance.constructed = (DayNightCycle.main.timePassedAsFloat - startedTime) / buildTime;
                }

                if (__instance.constructed >= 1f)
                {
                    CrafterTimes.Remove(uniqueId);
                }

                Shader.SetGlobalFloat(ShaderPropertyID._SubConstructProgress, __instance.constructed);
            }

            return false;
        }

        /**
         *
         * Sınıf id'sini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::VFXConstructing __instance)
        {
            return Network.Identifier.GetIdentityId(__instance.gameObject);
        }
    }
}

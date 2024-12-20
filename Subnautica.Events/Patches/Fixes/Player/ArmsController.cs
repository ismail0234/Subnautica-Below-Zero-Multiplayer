namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class ArmsController
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.OnToolUseAnim))]
        private static bool OnToolUseAnim(global::ArmsController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.player != null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.OnToolAnimDraw))]
        private static bool OnToolAnimDraw(global::ArmsController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.player != null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.OnToolAnimHolster))]
        private static bool OnToolAnimHolster(global::ArmsController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.player != null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.OnToolBleederHitAnim))]
        private static bool OnToolBleederHitAnim(global::ArmsController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.player != null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.BashHit))]
        private static bool BashHit(global::ArmsController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.player != null;
        }
    }
}

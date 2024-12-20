namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    [HarmonyPatch]
    public static class PlayerAnimationEventManager
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerAnimationEventManager), nameof(global::PlayerAnimationEventManager.DeathCut))]
        private static bool DeathCut(global::PlayerAnimationEventManager __instance)
        {
            return !Network.IsMultiplayerActive || !__instance.name.IsMultiplayerPlayer();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PlayerAnimationEventManager), nameof(global::PlayerAnimationEventManager.DeathFade))]
        private static bool DeathFade(global::PlayerAnimationEventManager __instance)
        {
            return !Network.IsMultiplayerActive || !__instance.name.IsMultiplayerPlayer();
        }
    }
}
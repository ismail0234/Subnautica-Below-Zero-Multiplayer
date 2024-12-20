namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class CrashHome
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CrashHome), nameof(global::CrashHome.Spawn))]
        private static bool Spawn(global::CrashHome __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CrashHome), nameof(global::CrashHome.Update))]
        private static bool Update(global::CrashHome __instance)
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CrashHome), nameof(global::CrashHome.Start))]
        private static void Start(global::CrashHome __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.prevClosed = true;
                __instance.animator.SetBool(AnimatorHashID.attacking, true);
            }
        }
    }
}

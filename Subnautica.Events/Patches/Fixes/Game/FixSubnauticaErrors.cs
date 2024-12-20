namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    [HarmonyPatch(typeof(global::SentrySdk), nameof(global::SentrySdk.OnLogMessageReceived))]
    public static class FixSubnauticaErrors
    {
        private static void Prefix(global::SentrySdk __instance)
        {
            if (__instance._initialized)
            {
                __instance._initialized = false;

                UnityEngine.Object.Destroy(__instance, 1f);
            }
        }
    }
}

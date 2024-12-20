namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::SplineFollowing), nameof(global::SplineFollowing.ManagedUpdate))]
    public class SplineFollowing
    {
        private static void Prefix(global::SplineFollowing __instance)
        {
            if (Network.IsMultiplayerActive && __instance.respectLOD)
            {
                __instance.respectLOD = false;
            }
        }
    }
}

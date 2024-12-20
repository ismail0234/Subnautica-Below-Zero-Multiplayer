namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::LaserCutter), nameof(global::LaserCutter.LaserCut))]
    public static class SealedObject
    {
        private static bool Prefix(global::LaserCutter __instance)
        {
            if (Network.IsMultiplayerActive && __instance.IsActiveCuttingTargetCanBeOpened())
            {
                if (__instance.activeCuttingTarget)
                {
                    if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.activeCuttingTarget.gameObject, false), true))
                    {
                        __instance.StopLaserCuttingFX();

                        Interact.ShowUseDenyMessage();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
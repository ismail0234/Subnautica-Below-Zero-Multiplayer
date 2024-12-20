namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::GroundMotor), nameof(global::GroundMotor.UpdateFunction))]
    public class MovingPlatform
    {
        private static void Prefix(global::GroundMotor __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.movingPlatform.hitPlatform != __instance.movingPlatform.activePlatform)
                {
                    __instance.movingPlatform.hitPlatform    = null;
                    __instance.movingPlatform.activePlatform = null;
                }
            }
        }
    }
}

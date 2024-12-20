namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class SeaTruckSegment
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.UpdateKinematicState))]
        private static bool Prefix(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive && __instance.isDocked)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.StartCreatureAttack))]
        private static bool StartCreatureAttack(global::SeaTruckSegment __instance, ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.underCreatureAttack = true;
            __instance.motor.OnCreatureAttackStart();
            __result = true;
            return false;
        }
    }
}

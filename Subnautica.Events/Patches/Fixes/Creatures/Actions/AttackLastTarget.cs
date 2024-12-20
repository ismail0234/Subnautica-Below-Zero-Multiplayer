namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public class AttackLastTarget
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::AttackLastTarget), nameof(global::AttackLastTarget.CanAttackTarget))]
        private static bool CanAttackTarget(global::AttackLastTarget __instance, GameObject target, ref bool __result)
        {
            if (target == null)
            {
                return false;
            }

            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            __result = false;

            if (target == null)
            {
                return false;
            }

            /*
            if (creature.IsFriendlyTo(target))
            {
                return false;
            }
            */

            if (target.GetTechType() != TechType.Player)
            {
                var component = target.GetComponent<LiveMixin>();
                if (!component || !component.IsAlive())
                {
                    return false;
                }
            }

            var player = ZeroPlayer.GetPlayerByGameObject(target);
            if (player != null && !player.CanBeAttacked())
            {
                return false;
            }

            if (GameModeManager.HasNoCreatureAggression() && (target.GetComponent<global::Vehicle>() || target.GetComponent<SeaTruckSegment>()))
            {
                return false;
            }

            if (target.GetComponent<SeaTruckMotor>() != null)
            {
                var vehicle = ZeroPlayer.GetPlayerByVehicleGameObject(target);
                if (vehicle == null)
                {
                    return false;
                }
            }

            var isUnderWater = target.transform.position.y < 0f;
            if (__instance.ignoreAboveWaterTargets && !isUnderWater)
            {
                return false;
            }

            if (__instance.ignoreUnderWaterTargets && isUnderWater)
            {
                return false;
            }

            __result = true;
            return false;
        }
    }
}
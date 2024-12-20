namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using Subnautica.API.Features;

    using HarmonyLib;

    using UnityEngine;

    [HarmonyPatch]
    public class MeleeAttack
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MeleeAttack), nameof(global::MeleeAttack.CanDealDamageTo))]
        private static bool CanDealDamageTo(global::MeleeAttack __instance, ref bool __result, GameObject target)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = false;

            var player = ZeroPlayer.GetPlayerByGameObject(target);
            if (player != null && !player.CanBeAttacked())
            {
                return false;
            }

            if (__instance.biteOnlyCurrentTarget && target != __instance.lastTarget.target)
            {
                return false;
            }

            if ((!__instance.canBitePlayer || player == null) && (!__instance.canBiteCreature || target.GetComponent<global::Creature>() == null) && (!__instance.canBiteVehicle || !__instance.IsValidVehicle(target)))
            {
                return false;
            }

            var direction = target.transform.position - __instance.transform.position;
            var magnitude = direction.magnitude;
            int num = UWE.Utils.RaycastIntoSharedBuffer(__instance.transform.position, direction, magnitude, -5, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < num; i++)
            {
                var collider   = UWE.Utils.sharedHitBuffer[i].collider;
                var gameObject = ((collider.attachedRigidbody != null) ? collider.attachedRigidbody.gameObject : collider.gameObject);
                var component2 = gameObject.GetComponent<global::ICreatureTargetProxy>();
                if (component2 != null && !component2.Equals(null))
                {
                    gameObject = component2.GetTarget();
                }

                if (gameObject != target && gameObject != __instance.gameObject && gameObject.GetComponent<global::Creature>() == null)
                {
                    return false;
                }
            }

            __result = true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MeleeAttack), nameof(global::MeleeAttack.IsValidVehicle))]
        private static bool IsValidVehicle(global::MeleeAttack __instance, ref bool __result, GameObject target)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = false;

            if (GameModeManager.HasNoCreatureAggression())
            {
                return false;
            }

            __result = ZeroPlayer.GetPlayerByVehicleGameObject(target) != null;
            return false;
        }
    }
}

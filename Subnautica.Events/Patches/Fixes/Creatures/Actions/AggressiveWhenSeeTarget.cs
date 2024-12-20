namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [HarmonyPatch(typeof(global::AggressiveWhenSeeTarget), nameof(global::AggressiveWhenSeeTarget.IsTargetValid), new Type[] { typeof(GameObject) })]
    public class AggressiveWhenSeeTarget
    {
        private static bool Prefix(global::AggressiveWhenSeeTarget __instance, GameObject target, ref bool __result)
        {
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

            var player = ZeroPlayer.GetPlayerByGameObject(target);
            if (player != null && !player.CanBeAttacked())
            {
                return false;
            }

            if (GameModeManager.HasNoCreatureAggression() && (target.GetComponent<global::Vehicle>() || target.GetComponent<global::SeaTruckSegment>()))
            {
                return false;
            }

            if (__instance.ignoreSameKind && target.GetTechType() == __instance.myTechType)
            {
                return false;
            }

            if (__instance.ignoreFrozen)
            {
                var component = target.GetComponent<FrozenMixin>();
                if (component != null && component.IsFrozen())
                {
                    return false;
                }
            }

            if (Vector3.Distance(target.transform.position, __instance.transform.position) > __instance.maxRangeScalar)
            {
                return false;
            }

            if (__instance.leashDistance > 0f && ZeroVector3.Distance(__instance.creature.GetLeashPosition(), target.transform.position) > __instance.leashDistance * __instance.leashDistance)
            {
                return false;
            }

            if (!Mathf.Approximately(__instance.minimumVelocity, 0f))
            {
                if (player == null)
                {
                    var componentInChildren = target.GetComponentInChildren<Rigidbody>();
                    if (componentInChildren != null && componentInChildren.velocity.magnitude <= __instance.minimumVelocity)
                    {
                        return false;
                    }
                }
                else
                {
                    if (player.GetVelocity().magnitude <= __instance.minimumVelocity)
                    {
                        return false;
                    }
                }
            }

            __result = __instance.creature.GetCanSeeObject(target);
            return false;
        }
    }
}
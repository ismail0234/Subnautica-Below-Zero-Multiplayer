namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public class AggressiveWhenSeePlayer
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::AggressiveWhenSeePlayer), nameof(global::AggressiveWhenSeePlayer.GetAggressionTarget))]
        private static bool GetAggressionTarget(global::AggressiveWhenSeePlayer __instance, ref GameObject __result)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            if (Time.time > __instance.timeLastPlayerAttack + __instance.playerAttackInterval * GameModeManager.GetReverseCreatureAggressionModifier())
            {
                foreach (var player in ZeroPlayer.GetAllPlayers())
                {
                    if (__instance.IsTargetValid(player.PlayerModel))
                    {
                        __result = player.PlayerModel;
                        return false;
                    }
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::AggressiveWhenSeePlayer), nameof(global::AggressiveWhenSeePlayer.OnMeleeAttack))]
        private static bool OnMeleeAttack(global::AggressiveWhenSeePlayer __instance, GameObject target)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            if (ZeroPlayer.GetPlayerByGameObject(target) != null)
            {
                __instance.timeLastPlayerAttack = Time.time;
                return false;
            }

            return true;
        }
    }
}

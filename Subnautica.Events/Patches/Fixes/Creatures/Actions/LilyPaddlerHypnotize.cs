namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [HarmonyPatch]
    public class LilyPaddlerHypnotize
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::LilyPaddlerHypnotize), nameof(global::LilyPaddlerHypnotize.ManagedUpdate))]
        private static bool ManagedUpdate()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::LilyPaddlerHypnotize), nameof(global::LilyPaddlerHypnotize.IsValidTarget))]
        private static bool IsValidTarget(global::LilyPaddlerHypnotize __instance, GameObject target, ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = false;

            if (target == null)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerByGameObject(target);
            if (player == null)
            {
                return false;
            }

            if (target != __instance.currentTarget && global::LilyPaddlerHypnotize.allTargets.Contains(target))
            {
                return false;
            }

            if (!player.CanHypnotizePlayer())
            {
                return false;
            }

            if (ZeroVector3.Distance(target.transform.position, __instance.transform.position) > __instance.maxDistance * __instance.maxDistance)
            {
                return false;
            }

            __result = player.LooksAtMe(__instance.gameObject);
            return false;
        }
    }
}

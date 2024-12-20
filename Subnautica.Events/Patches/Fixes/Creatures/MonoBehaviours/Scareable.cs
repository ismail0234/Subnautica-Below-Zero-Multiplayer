namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Scareable), nameof(global::Scareable.IsTargetValid))]
    public class Scareable
    {
        private static bool Prefix(global::Scareable __instance, IEcoTarget ecoTarget, ref bool __result)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            __result = false;

            var target = ecoTarget.GetGameObject();
            if (target == null)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerByGameObject(target);
            if (player != null && !player.CanBeAttacked())
            {
                return false;
            }

            /*
                if (creature.IsFriendlyTo(gameObject))
                {
                    return false;
                }
            */

            if (ZeroVector3.Distance(ecoTarget.GetPosition(), __instance.transform.position) > __instance.maxRangeScalar * __instance.maxRangeScalar)
            {
                return false;
            }

            var component = target.GetComponent<Rigidbody>();
            if (!component || component.mass < __instance.minMass)
            {
                return false;
            }

            __result = true;
            return false;
        }
    }
}

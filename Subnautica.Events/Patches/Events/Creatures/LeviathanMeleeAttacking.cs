namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::LeviathanMeleeAttack), nameof(global::LeviathanMeleeAttack.OnTouch))]
    public class LeviathanMeleeAttacking
    {
        private static bool Prefix(global::LeviathanMeleeAttack __instance, Collider collider)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            if (__instance.cinematicActive || !__instance.enabled || !__instance.liveMixin.IsAlive() || __instance.creature.Scared.Value > __instance.scareThreshold)
            {
                return false;
            }

            var target = __instance.GetTarget(collider);

            if (!__instance.CanBite(target))
            {
                return false;
            }

            try
            {
                CreatureLeviathanMeleeAttackingEventArgs args = new CreatureLeviathanMeleeAttackingEventArgs(__instance, target);

                Handlers.Creatures.OnLeviathanMeleeAttacking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"LeviathanMeleeAttacking.Prefix: {e}\n{e.StackTrace}");
            }

            return false;
        }
    }
}

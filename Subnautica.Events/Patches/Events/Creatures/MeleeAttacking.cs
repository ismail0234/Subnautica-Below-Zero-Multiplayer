namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::MeleeAttack), nameof(global::MeleeAttack.OnTouch))]
    public class MeleeAttacking
    {
        private static bool Prefix(global::MeleeAttack __instance, Collider collider)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized())
            {
                return true;
            }

            if (!__instance.enabled || !__instance.liveMixin.IsAlive())
            {
                return false;
            }

            var target = __instance.GetTarget(collider);
            if (target == __instance.gameObject)
            {
                return false;
            }

            if (__instance.ignoreSameKind && global::CreatureData.GetCreatureType(__instance.gameObject) == global::CreatureData.GetCreatureType(target))
            {
                return false;
            }

            if (__instance.canBeFed)
            {
                var player = ZeroPlayer.GetPlayerByGameObject(target);
                if (player != null && player.CanBeAttacked())
                {
                    // ErrorMessage.AddMessage("CANBEFED NOT SYNCED!");
                    // return false;
                }
            }

            if (!__instance.CanBite(target))
            {
                return false;
            }

            try
            {
                CreatureMeleeAttackingEventArgs args = new CreatureMeleeAttackingEventArgs(__instance, target);

                Handlers.Creatures.OnMeleeAttacking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MeleeAttacking.Prefix: {e}\n{e.StackTrace}");
                return false;
            }
        }
    }
}

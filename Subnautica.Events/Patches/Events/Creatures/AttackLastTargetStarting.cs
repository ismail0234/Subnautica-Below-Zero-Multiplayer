namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::AttackLastTarget), nameof(global::AttackLastTarget.StartPerform))]
    public class AttackLastTargetStarting
    {
        private static bool Prefix(global::AttackLastTarget __instance)
        {
            if (!Network.IsMultiplayerActive || !__instance.creature.IsSynchronized() || EventBlocker.IsEventBlocked(ProcessType.CreatureAttackLastTarget))
            {
                return true;
            }

            try
            {
                CreatureAttackLastTargetStartingEventArgs args = new CreatureAttackLastTargetStartingEventArgs(__instance.creature, __instance.creature.gameObject.GetIdentityId(), __instance.currentTarget, __instance.minAttackDuration, __instance.maxAttackDuration);

                Handlers.Creatures.OnCreatureAttackLastTargetStarting(args);

                if (args.IsAllowed)
                {
                    return true;
                }

                __instance.StopAttack();
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"AttackLastTargetStarting.Prefix: {e}\n{e.StackTrace}");
            }

            return false;
        }
    }
}

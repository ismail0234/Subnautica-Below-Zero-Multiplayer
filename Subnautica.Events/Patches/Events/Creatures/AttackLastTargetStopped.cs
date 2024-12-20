namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::AttackLastTarget), nameof(global::AttackLastTarget.StopPerform))]
    public class AttackLastTargetStopped
    {
        private static void Prefix(global::AttackLastTarget __instance)
        {
            if (Network.IsMultiplayerActive && __instance.creature.IsSynchronized())
            {
                try
                {
                    CreatureAttackLastTargetStoppedEventArgs args = new CreatureAttackLastTargetStoppedEventArgs(__instance.creature, __instance.creature.gameObject.GetIdentityId(), __instance.creature.GetAnimator().GetBool(AnimatorHashID.attacking));

                    Handlers.Creatures.OnCreatureAttackLastTargetStopped(args);
                }
                catch (Exception e)
                {
                    Log.Error($"AttackLastTargetStopped.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

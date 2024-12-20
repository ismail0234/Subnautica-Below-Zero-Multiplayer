namespace Subnautica.Events.Patches.Fixes.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::SpikeyTrap), nameof(global::SpikeyTrap.FinishAttack))]
    public class Temp_SpikeyTrap
    {
        private static bool Prefix(global::SpikeyTrap __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.currentTarget != null && __instance.currentTarget.targetGO != null)
            {
                __instance.currentTarget.targetGO.transform.SetParent(null);
                LiveMixin component = __instance.currentTarget.targetGO.GetComponent<LiveMixin>();
                if (component != null && component.IsAlive())
                {
                    component.TakeDamage(9999f);
                }

                __instance.ResetCurrentTarget();
            }

            __instance.ResetActiveArm();
            __instance.timeNextHunt = Time.time + __instance.eatInterval;
            __instance.OnStateTransitionStart(0.3f);
            __instance.SetState(SpikeyTrap.State.Rest);
            return false;
        }
    }
}

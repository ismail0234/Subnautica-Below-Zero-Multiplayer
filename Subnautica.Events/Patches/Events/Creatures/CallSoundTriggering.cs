namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;
    using System.Collections;

    [HarmonyPatch(typeof(global::CreatureCallSound), nameof(global::CreatureCallSound.TriggerCallAsync))]
    public class CallSoundTriggering
    {
        private static IEnumerator Postfix(IEnumerator values, global::CreatureCallSound __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                CreatureCallSoundTriggeringEventArgs args = new CreatureCallSoundTriggeringEventArgs(__instance.GetComponentInParent<global::Creature>().gameObject.GetIdentityId(), __instance.callVariants > 1 ? (byte) UnityEngine.Random.Range(1, __instance.callVariants + 1) : (byte) 0, __instance.animation);

                try
                {
                    Handlers.Creatures.OnCallSoundTriggering(args);
                }
                catch (Exception e)
                {
                    Log.Error($"CallSoundTriggering.Postfix: {e}\n{e.StackTrace}");
                }

                if (args.IsAllowed)
                {
                    yield return values;
                }
            }
            else
            {
                yield return values;
            }
        }
    }
}

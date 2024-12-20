namespace Subnautica.Events.Patches.Events.Story
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::OnTouch), nameof(global::OnTouch.OnTriggerEnter))]
    public class ShieldBaseEnterTriggering
    {
        private static bool Prefix(global::OnTouch __instance, Collider collider)
        {
            if (Network.IsMultiplayerActive)
            {
                if (!__instance.RespondToCollider(collider))
                {
                    return false;
                }

                var prs = __instance.transform.parent?.parent?.gameObject.GetComponentInChildren<PowerRoomState>();
                if (prs == null)
                {
                    return true;
                }

                try
                {
                    ShieldBaseEnterTriggeringEventArgs args = new ShieldBaseEnterTriggeringEventArgs();

                    Handlers.Story.OnShieldBaseEnterTriggering(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ShieldBaseEnterTriggering.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}

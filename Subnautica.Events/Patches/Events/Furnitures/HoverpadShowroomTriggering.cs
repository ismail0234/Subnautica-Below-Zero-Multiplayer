namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch]
    public static class HoverpadShowroomTriggering
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverpadShowroomTrigger), nameof(global::HoverpadShowroomTrigger.OnTriggerEnter))]
        private static bool OnTriggerEnter(global::HoverpadShowroomTrigger __instance, Collider col)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!col.gameObject.Equals(global::Player.main.gameObject))
            {
                return false;
            }

            try
            {
                HoverpadShowroomTriggeringEventArgs args = new HoverpadShowroomTriggeringEventArgs(Network.Identifier.GetIdentityId(__instance.hoverpad.gameObject), true);

                Handlers.Furnitures.OnHoverpadShowroomTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverpadShowroomTriggering.OnTriggerEnter: {e}\n{e.StackTrace}");
                return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverpadShowroomTrigger), nameof(global::HoverpadShowroomTrigger.OnTriggerExit))]
        private static bool OnTriggerExit(global::HoverpadShowroomTrigger __instance, Collider col)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!col.gameObject.Equals(global::Player.main.gameObject))
            {
                return false;
            }

            try
            {
                HoverpadShowroomTriggeringEventArgs args = new HoverpadShowroomTriggeringEventArgs(Network.Identifier.GetIdentityId(__instance.hoverpad.gameObject), false);

                Handlers.Furnitures.OnHoverpadShowroomTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverpadShowroomTriggering.OnTriggerExit: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}

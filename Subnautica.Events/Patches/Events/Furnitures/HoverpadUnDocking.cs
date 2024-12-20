namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::HoverpadUndockTrigger), nameof(global::HoverpadUndockTrigger.OnHandClick))]
    public static class HoverpadUnDocking
    {
        private static bool Prefix(global::HoverpadUndockTrigger __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(TechType.Hoverbike))
            {
                return true;
            }

            try
            {
                HoverpadUnDockingEventArgs args = new HoverpadUnDockingEventArgs(Network.Identifier.GetIdentityId(__instance.hoverpad.gameObject), Network.Identifier.GetIdentityId(__instance.hoverpad.dockedBike.gameObject), __instance.hoverpad.dockedBike.transform.position, __instance.hoverpad.dockedBike.transform.rotation);

                Handlers.Furnitures.OnHoverpadUnDocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverpadUnDocking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}

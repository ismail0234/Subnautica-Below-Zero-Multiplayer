namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class BridgeTerminalClicking
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_GlacialBasinBridgeScreen), nameof(global::uGUI_GlacialBasinBridgeScreen.OnExtendClick))]
        private static bool uGUI_GlacialBasinBridgeScreen_OnExtendClick(global::uGUI_GlacialBasinBridgeScreen __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var bridge = __instance.GetComponentInParent<GlacialBasinBridgeController>();
            if (bridge == null)
            {
                return false;
            }

            try
            {

                BridgeTerminalClickingEventArgs args = new BridgeTerminalClickingEventArgs(Network.Identifier.GetIdentityId(bridge.gameObject), true, bridge.extendBridgeTimeline.duration);

                Handlers.Story.OnBridgeTerminalClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BridgeTerminalClicking.uGUI_GlacialBasinBridgeScreen_OnExtendClick: {e}\n{e.StackTrace}");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_GlacialBasinBridgeScreen), nameof(global::uGUI_GlacialBasinBridgeScreen.OnRetractClick))]
        private static bool uGUI_GlacialBasinBridgeScreen_OnRetractClick(global::uGUI_GlacialBasinBridgeScreen __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }
            
            var bridge = __instance.GetComponentInParent<GlacialBasinBridgeController>();
            if (bridge == null)
            {
                return false;
            }

            try
            {

                BridgeTerminalClickingEventArgs args = new BridgeTerminalClickingEventArgs(Network.Identifier.GetIdentityId(bridge.gameObject), false, bridge.retractBridgeTimeline.duration);

                Handlers.Story.OnBridgeTerminalClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BridgeTerminalClicking.uGUI_GlacialBasinBridgeScreen_OnRetractClick: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
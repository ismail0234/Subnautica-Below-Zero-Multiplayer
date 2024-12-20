namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::GlacialBasinBridgeController), nameof(global::GlacialBasinBridgeController.Start))]
    public static class BridgeInitialized
    {
        private static void Postfix(global::GlacialBasinBridgeController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    BridgeInitializedEventArgs args = new BridgeInitializedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                    Handlers.Story.OnBridgeInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"BridgeInitalized.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
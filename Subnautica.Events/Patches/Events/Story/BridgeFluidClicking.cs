namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::GlacialBasinBridgeController), nameof(global::GlacialBasinBridgeController.OnHandClick))]
    public static class BridgeFluidClicking
    {
        private static bool Prefix(global::GlacialBasinBridgeController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.itemInsertedStoryGoal.IsComplete() || global::Inventory.main.GetPickupCount(__instance.requiredTechType) <= 0)
            {
                return false;
            }

            try
            {
                BridgeFluidClickingEventArgs args = new BridgeFluidClickingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), __instance.itemInsertedStoryGoal.key);

                Handlers.Story.OnBridgeFluidClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BridgeFluidClicking.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::MobileExtractorMachine), nameof(global::MobileExtractorMachine.OnClickCanister))]
    public static class MobileExtractorMachineSampleAdding
    {
        private static bool Prefix(global::MobileExtractorMachine __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.hasSample || global::Inventory.main.GetPickupCount(__instance.sampleTechType) <= 0)
            {
                return false;
            }

            try
            {
                MobileExtractorMachineSampleAddingEventArgs args = new MobileExtractorMachineSampleAddingEventArgs();

                Handlers.Story.OnMobileExtractorMachineSampleAdding(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MobileExtractorMachineSampleAdding.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
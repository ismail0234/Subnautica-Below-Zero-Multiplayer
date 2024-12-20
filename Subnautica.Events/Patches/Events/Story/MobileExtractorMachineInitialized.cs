namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using System;

    [HarmonyPatch(typeof(global::MobileExtractorMachine), nameof(global::MobileExtractorMachine.Start))]
    public static class MobileExtractorMachineInitialized
    {
        private static void Postfix(global::MobileExtractorMachine __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.Story.OnMobileExtractorMachineInitialized();
                }
                catch (Exception e)
                {
                    Log.Error($"MobileExtractorMachineInitialized.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
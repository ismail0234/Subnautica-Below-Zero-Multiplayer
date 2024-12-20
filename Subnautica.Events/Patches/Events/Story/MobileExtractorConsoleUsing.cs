namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::MobileExtractorConsole), nameof(global::MobileExtractorConsole.OnInjectClick))]
    public static class MobileExtractorConsoleUsing
    {
        private static bool Prefix(global::MobileExtractorConsole __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.GetState(MobileExtractorMachine.main) != MobileExtractorConsole.State.Ready)
            {
                return false;
            }

            try
            {
                MobileExtractorConsoleUsingEventArgs args = new MobileExtractorConsoleUsingEventArgs();

                Handlers.Story.OnMobileExtractorConsoleUsing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MobileExtractorConsoleUsing.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.NotifyRemove))]
    public static class ScannerCompleted
    {
        private static void Postfix(PDAScanner.Entry entry)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    ScannerCompletedEventArgs args = new ScannerCompletedEventArgs(entry.techType);

                    Handlers.PDA.OnScannerCompleted(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ScannerCompleted.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
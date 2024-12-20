namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.NotifyProgress))]
    public static class TechnologyFragmentAdded
    {
        private static void Postfix(PDAScanner.Entry entry)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    var entryData = PDAScanner.GetEntryData(entry.techType);

                    TechnologyFragmentAddedEventArgs args = new TechnologyFragmentAddedEventArgs(GetUniqueId(), entry.techType, entry.unlocked, entryData.totalFragments);

                    Handlers.PDA.OnTechnologyFragmentAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TechnologyFragmentAdded.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * UniqueId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId()
        {
            var result = PDAScanner.CanScan(PDAScanner.scanTarget);
            if (result == PDAScanner.Result.Processed)
            {
                return PDAScanner.scanTarget.uid;
            }

            return null;
        }
    }
}

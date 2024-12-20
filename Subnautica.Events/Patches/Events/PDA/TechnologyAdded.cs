namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(KnownTech), nameof(KnownTech.NotifyAdd))]
    public static class TechnologyAdded
    {
        private static void Postfix(TechType techType, bool verbose)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    TechnologyAddedEventArgs args = new TechnologyAddedEventArgs(techType, verbose);

                    Handlers.PDA.OnTechnologyAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TechnologyScanCompleted.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

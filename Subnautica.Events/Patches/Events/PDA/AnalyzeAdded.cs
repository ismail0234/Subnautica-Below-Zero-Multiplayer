namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    
    using System;

    [HarmonyPatch(typeof(KnownTech), nameof(KnownTech.NotifyAnalyze))]
    public static class AnalyzeAdded
    {
        public static void Postfix(KnownTech.AnalysisTech analysis, bool verbose)
        {
            if (Network.IsMultiplayerActive)
            {
                if (GameModeManager.HaveGameOptionsSet && !GameModeManager.GetOption<bool>(GameOption.TechRequiresUnlocking))
                {
                    return;
                }

                try
                {
                    TechAnalyzeAddedEventArgs args = new TechAnalyzeAddedEventArgs(analysis.techType, verbose);

                    Handlers.PDA.OnTechAnalyzeAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"AnalyzeAdded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
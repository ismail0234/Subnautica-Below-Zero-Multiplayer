namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(PDALog), nameof(PDALog.NotifyAdd))]
    public static class LogAdded
    {
        private static void Postfix(PDALog.Entry entry)
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    PDALogAddedEventArgs args = new PDALogAddedEventArgs(entry.data.key, entry.timestamp);

                    Handlers.PDA.OnLogAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"LogAdded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
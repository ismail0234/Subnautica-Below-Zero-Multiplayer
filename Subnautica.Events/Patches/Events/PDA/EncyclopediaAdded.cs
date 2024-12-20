namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(PDAEncyclopedia), nameof(PDAEncyclopedia.NotifyAdd))]
    public static class EncyclopediaAdded
    {
        public static void Postfix(CraftNode node, bool verbose)
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    EncyclopediaAddedEventArgs args = new EncyclopediaAddedEventArgs(node.id, verbose);

                    Handlers.PDA.OnEncyclopediaAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"EncyclopediaAdded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using System;

    [HarmonyPatch(typeof(CSVEntitySpawner), nameof(CSVEntitySpawner.ResetSpawner))]
    public class EntityDistributionLoaded
    {
        private static void Postfix()
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.Game.OnEntityDistributionLoaded();
                }
                catch (Exception e)
                {
                    Log.Error($"EntityDistributionLoaded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

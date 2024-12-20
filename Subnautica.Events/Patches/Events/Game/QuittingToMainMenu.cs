namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.QuitGame))]
    public class QuittingToMainMenu
    {
        private static bool Prefix(IngameMenu __instance, bool quitToDesktop)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                QuittingToMainMenuEventArgs args = new QuittingToMainMenuEventArgs(quitToDesktop);

                Handlers.Game.OnQuittingToMainMenu(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"QuittingToMainMenu.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}

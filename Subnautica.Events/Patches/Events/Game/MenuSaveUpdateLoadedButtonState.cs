namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(MainMenuLoadPanel), nameof(MainMenuLoadPanel.UpdateLoadButtonState))]
    public class MenuSaveUpdateLoadedButtonState
    {
        private static void Postfix(MainMenuLoadPanel __instance, MainMenuLoadButton lb)
        {
            try
            {
                MenuSaveUpdateLoadedButtonStateEventArgs args = new MenuSaveUpdateLoadedButtonStateEventArgs(lb);

                Handlers.Game.OnMenuSaveUpdateLoadedButtonState(args);
            }
            catch (Exception e)
            {
                Log.Error($"MenuSaveLoadButtonClicking.Prefix: {e}\n{e.StackTrace}");
            }
        }
    }
}

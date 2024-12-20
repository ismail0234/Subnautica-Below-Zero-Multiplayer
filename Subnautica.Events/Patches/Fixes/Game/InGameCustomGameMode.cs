namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(uGUI_OptionsPanel), nameof(uGUI_OptionsPanel.AddGameModeOptionsTab))]
    public class InGameCustomGameMode
    {
        private static bool Prefix()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

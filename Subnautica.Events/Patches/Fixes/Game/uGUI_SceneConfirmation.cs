namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::uGUI_SceneConfirmation), nameof(global::uGUI_SceneConfirmation.Show))]
    public class uGUI_SceneConfirmation
    {

        private static void Prefix()
        {
            ZeroModal.ApplyDefaultSettings();
        }
    }
}

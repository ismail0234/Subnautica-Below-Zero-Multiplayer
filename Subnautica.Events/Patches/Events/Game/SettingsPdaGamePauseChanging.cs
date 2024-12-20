namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(uGUI_OptionsPanel), nameof(uGUI_OptionsPanel.OnPDAPauseChanged))]
    public class SettingsPdaGamePauseChanging
    {
        private static bool Prefix(uGUI_OptionsPanel __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                SettingsPdaGamePauseChangingEventArgs args = new SettingsPdaGamePauseChangingEventArgs();

                Handlers.Game.OnSettingsPdaGamePauseChanging(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SettingsPdaGamePauseChanging.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }

}

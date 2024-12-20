namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(uGUI_OptionsPanel), nameof(uGUI_OptionsPanel.OnRunInBackgroundChanged))]
    public class SettingsRunInBackgroundChanging
    {
        private static bool Prefix(uGUI_OptionsPanel __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                SettingsRunInBackgroundChangingEventArgs args = new SettingsRunInBackgroundChangingEventArgs();

                Handlers.Game.OnSettingsRunInBackgroundChanging(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SettingsRunInBackgroundChanging.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}

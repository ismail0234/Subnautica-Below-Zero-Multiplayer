namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::uGUI_PlayerSleep), nameof(global::uGUI_PlayerSleep.BeginFadeOut))]
    public static class SleepScreenStopingStarted
    {   
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::uGUI_PlayerSleep __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.Player.OnSleepScreenStopingStarted();
                }
                catch (Exception e)
                {
                    Log.Error($"SleepScreenStopingStarted.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}
namespace Subnautica.Events.Patches.Fixes.Game
{
    using System;

    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch]
    public static class Weather
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::WeatherManager), nameof(global::WeatherManager.ExtendWeatherTimeline))]
        private static bool WeatherManager_ExtendWeatherTimeline()
        {
            if (Network.IsMultiplayerActive && World.IsLoaded && !BelowZeroEndGame.isActive)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::WeatherManager), nameof(global::WeatherManager.AdvanceSimulation))]
        private static bool WeatherManager_AdvanceSimulation()
        {
            if (Network.IsMultiplayerActive && World.IsLoaded && !BelowZeroEndGame.isActive)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::WeatherManager), nameof(global::WeatherManager.ActivateWeatherProfile), new Type[] { typeof(WeatherProfile) })]
        private static bool WeatherManager_ActivateWeatherProfile(global::WeatherManager __instance, WeatherProfile profile)
        {
            if (Network.IsMultiplayerActive && World.IsLoaded)
            {
                __instance.activeScriptedWeather = null;
                __instance.currentWeatherTrigger = null;
                __instance.savedWeatherEventId   = string.Empty;
                __instance.currentWeatherProfile = profile;
                return false;
            }

            return true;
        }
    }
}

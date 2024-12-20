namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::WeatherManager), nameof(global::WeatherManager.ToString))]
    public static class WeatherManager
    {
        private static bool Prefix(global::WeatherManager __instance, ref string __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.activeScriptedWeather == null)
            {
                __result = "";
                return false;
            }

            if (__instance.currentWeatherEvent == null)
            {
                __result = $"SCRIPTED WEATHER: {__instance.activeScriptedWeather.ToString()}";
                return false;
            }

            return true;
        }
    }
}

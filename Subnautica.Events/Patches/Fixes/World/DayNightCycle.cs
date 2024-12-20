namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::DayNightCycle), nameof(global::DayNightCycle.SetDayNightTime))]
    public class DayNightCycle
    {
        private static bool Prefix()
        {
            if (BelowZeroEndGame.isActive)
            {
                return true;
            }

            return !Network.IsMultiplayerActive;
        }
    }
}
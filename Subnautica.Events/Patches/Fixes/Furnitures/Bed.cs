namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Bed
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.Update))]
        private static bool Update(global::Bed __instance)
        {
            if (Network.IsMultiplayerActive && __instance.inUseMode == global::Bed.InUseMode.Sleeping)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::DayNightCycle), nameof(global::DayNightCycle.SkipTime))]
        private static bool SkipTime()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.ExitInUseMode))]
        private static void ExitInUseModePrefix(global::Player player)
        {
            if (Network.IsMultiplayerActive)
            {
                PlayerTimeLastSleep = player.timeLastSleep;
            }
        }     

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.ExitInUseMode))]
        private static void ExitInUseModePostfix(global::Player player)
        {
            if (Network.IsMultiplayerActive)
            {
                player.timeLastSleep = PlayerTimeLastSleep;
            }
        } 

        private static float PlayerTimeLastSleep { get; set; } = -1f;
    }
}

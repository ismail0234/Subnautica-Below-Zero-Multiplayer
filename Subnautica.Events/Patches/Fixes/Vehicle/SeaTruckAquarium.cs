namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::SeaTruckAquarium), nameof(global::SeaTruckAquarium.OnTriggerEnter))]
    public class SeaTruckAquarium
    {
        private static bool Prefix()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

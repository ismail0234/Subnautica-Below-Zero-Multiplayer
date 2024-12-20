namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::SpyPenguinRemoteManager), nameof(global::SpyPenguinRemoteManager.CyclePenguin))]
    public class SpyPenguin
    {
        private static bool Prefix()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    [HarmonyPatch(typeof(global::Respawn), nameof(global::Respawn.Start))]
    public static class Respawn
    {
        private static bool Prefix()
        {
            return !API.Features.Network.IsMultiplayerActive;
        }
    }
}

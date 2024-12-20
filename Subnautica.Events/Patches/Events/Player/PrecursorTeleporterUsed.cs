namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::PrecursorTeleporter), nameof(global::PrecursorTeleporter.TeleportCoroutine))]
    public class PrecursorTeleporterUsed
    {
        private static void Prefix(global::PrecursorTeleporter __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.Player.OnPrecursorTeleporterUsed();
                }
                catch (Exception e)
                {
                    Log.Error($"PrecursorTeleporterUsed.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
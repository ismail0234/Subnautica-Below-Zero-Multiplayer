namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PrecursorTeleporter), nameof(global::PrecursorTeleporter.OnEnable))]
    public static class TeleporterInitialized
    {
        private static void Prefix(global::PrecursorTeleporter __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    TeleporterInitializedEventArgs args = new TeleporterInitializedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), __instance.teleporterIdentifier, !__instance.GetComponentInParent<PrecursorTeleporterActivationTerminal>());

                    Handlers.World.OnTeleporterInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TeleporterEnabled.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
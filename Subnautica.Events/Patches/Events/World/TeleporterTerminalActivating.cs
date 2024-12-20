namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PrecursorTeleporterActivationTerminal), nameof(global::PrecursorTeleporterActivationTerminal.OnProxyHandClick))]
    public static class TeleporterTerminalActivating
    {
        private static bool Prefix(global::PrecursorTeleporterActivationTerminal __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (EventBlocker.IsEventBlocked(ProcessType.PrecursorTeleporter))
            {
                return true;
            }

            if (__instance.unlocked || !global::Inventory.main.container.Contains(TechType.PrecursorIonCrystal))
            {
                return false;
            }

            try
            {
                TeleporterTerminalActivatingEventArgs args = new TeleporterTerminalActivatingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), __instance.GetComponentInChildren<PrecursorTeleporter>()?.teleporterIdentifier);

                Handlers.World.OnTeleporterTerminalActivating(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"TeleporterTerminalActivating.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
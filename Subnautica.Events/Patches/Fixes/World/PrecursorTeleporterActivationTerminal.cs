namespace Subnautica.Events.Patches.Fixes.World
{
    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch(typeof(global::PrecursorTeleporterActivationTerminal), nameof(global::PrecursorTeleporterActivationTerminal.Start))]
    public class PrecursorTeleporterActivationTerminal
    {
        private static void Postfix(global::PrecursorTeleporterActivationTerminal __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (TeleporterManager.GetTeleporterActive(Network.Identifier.GetIdentityId(__instance.gameObject, false)))
                {
                    __instance.unlocked = true;
                }
            }
        }
    }
}

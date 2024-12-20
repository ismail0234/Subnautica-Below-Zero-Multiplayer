namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch]
    public class TeleportationToolUsed
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::TeleportationTool), nameof(global::TeleportationTool.StartTeleportSequence))]
        private static void TeleportationTool_StartTeleportSequence(global::TeleportationTool __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    TeleportationToolUsedEventArgs args = new TeleportationToolUsedEventArgs(null);

                    Handlers.Items.OnTeleportationToolUsed(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TeleportationTool_StartTeleportSequence: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckTeleporter), nameof(global::SeaTruckTeleporter.PlayTeleportAnimation))]
        private static void TeleportationTool_MovePlayer(global::SeaTruckTeleporter __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    TeleportationToolUsedEventArgs args = new TeleportationToolUsedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false));

                    Handlers.Items.OnTeleportationToolUsed(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TeleportationTool_MovePlayer: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.ExitCurrentInterior))]
    public static class ExitedInterior
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Player __instance)
        {
            if (Network.IsMultiplayerActive && !Tools.IsInStackTrace("SeaTruckSegment_OnKill_ExitInterior"))
            {
                if (__instance.currentInterior == null || __instance.currentInterior is SubRoot)
                {
                    return;
                }

                try
                {
                    PlayerExitedInteriorEventArgs args = new PlayerExitedInteriorEventArgs(Network.Identifier.GetIdentityId(__instance.currentInterior.GetGameObject().gameObject));

                    Handlers.Player.OnExitedInterior(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ExitedInterior.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
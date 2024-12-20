namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SubRoot), nameof(global::SubRoot.OnPlayerEntered))]
    public static class EnteredSubroot
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::SubRoot __instance)
        {
            if (Network.IsMultiplayerActive && __instance.isBase)
            {
                try
                {
                    PlayerBaseEnteredEventArgs args = new PlayerBaseEnteredEventArgs(__instance.gameObject.GetIdentityId());

                    Handlers.Player.OnPlayerBaseEntered(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ExitedSubroot.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
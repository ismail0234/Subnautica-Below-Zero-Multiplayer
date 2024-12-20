namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SubRoot), nameof(global::SubRoot.OnPlayerExited))]
    public static class ExitedSubroot
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
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    if (__instance.isBase)
                    {
                        PlayerBaseExitedEventArgs args = new PlayerBaseExitedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                        Handlers.Player.OnPlayerBaseExited(args);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"ExitedSubroot.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
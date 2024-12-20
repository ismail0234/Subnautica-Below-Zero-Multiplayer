namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PingInstance), nameof(global::PingInstance.SetVisible))]
    public class PingVisibilityChanged
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::PingInstance __instance, bool value)
        {
            if (Network.IsMultiplayerActive && __instance.visible != value)
            {
                try
                {
                    PlayerPingVisibilityChangedEventArgs args = new PlayerPingVisibilityChangedEventArgs(__instance.Id, value);

                    Handlers.Player.OnPingVisibilityChanged(args);
                }
                catch (Exception e)
                {
                    Log.Error($"BeaconVisibilityChanged.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
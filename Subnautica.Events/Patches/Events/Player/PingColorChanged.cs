namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PingInstance), nameof(global::PingInstance.SetColor))]
    public class PingColorChanged
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::PingInstance __instance, int index)
        {
            if (Network.IsMultiplayerActive)
            {
                if (index >= PingManager.colorOptions.Length)
                {
                    index = 0;
                }

                if (__instance.colorIndex != index)
                {
                    try
                    {
                        PlayerPingColorChangedEventArgs args = new PlayerPingColorChangedEventArgs(__instance._id, index);

                        Handlers.Player.OnPingColorChanged(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"BeaconColorChanged.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::uGUI_PlayerSleep), nameof(global::uGUI_PlayerSleep.Update))]
    public static class SleepScreenStartingCompleted
    {   
        /**
         *
         * Siyah Renk
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Color BlackColor { get; set; } = new Color(0.0f, 0.0f, 0.0f, 1f);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::uGUI_PlayerSleep __instance)
        {
            if (Network.IsMultiplayerActive && __instance.state == uGUI_PlayerSleep.State.FadeIn)
            {
                var color = __instance.blackOverlay.color;

                __instance.blackOverlay.color   = Color.Lerp(color, BlackColor, Time.deltaTime * __instance.fadeInSpeed);
                __instance.blackOverlay.enabled = true;
                
                if (color.a > 0.980000019073486)
                {
                    __instance.blackOverlay.color = BlackColor;
                    __instance.state = uGUI_PlayerSleep.State.Enabled;

                    try
                    {
                        Handlers.Player.OnSleepScreenStartingCompleted();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"SleepScreenStartingCompleted.Prefix: {e}\n{e.StackTrace}");
                    }
                }

                return false;
            }

            return true;
        }
    }
}

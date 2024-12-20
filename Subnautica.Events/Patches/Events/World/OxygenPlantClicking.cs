namespace Subnautica.Events.Patches.Events.World
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::OxygenPlant), nameof(global::OxygenPlant.OnHandClick))]
    public static class OxygenPlantClicking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::OxygenPlant __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.allowEarlyReplenishment && __instance.GetProgress() < 1.0f)
            {
                return false;
            }

            float seconds = GetSeconds(__instance);
            if (seconds <= 0.0f)
            {
                return false;
            }

            float startedTime = GetStartedTime((float) __instance.duration, ((__instance.GetProgress() * __instance.capacity) - seconds) / __instance.capacity);

            try
            {
                OxygenPlantClickingEventArgs args = new OxygenPlantClickingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), startedTime);

                Handlers.World.OnOxygenPlantClicking(args);
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"OxygenPlantClicking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }

        /**
         *
         * Eklenecek saniyeyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static float GetSeconds(global::OxygenPlant __instance)
        {
            return Mathf.Min(__instance.GetProgress() * __instance.capacity, global::Player.main.oxygenMgr.GetOxygenCapacity() - global::Player.main.oxygenMgr.GetOxygenAvailable());
        }

        /**
         *
         * Başlangıç zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static float GetStartedTime(float duration, float value)
        {
            return DayNightCycle.main.timePassedAsFloat - duration * Mathf.Clamp01(value);
        }
    }
}
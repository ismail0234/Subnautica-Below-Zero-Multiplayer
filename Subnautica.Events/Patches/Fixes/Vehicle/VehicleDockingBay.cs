namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.LateUpdate))]
    public class VehicleDockingBay
    {
        private static bool Prefix(global::VehicleDockingBay __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Time.time < __instance.timeLastCharged + 1.0f)
            {
                return false;
            }

            __instance.timeLastCharged = Time.time;

            if (__instance.dockedObject)
            {
                if (!__instance.powerRelay)
                {
                    __instance.powerRelay = __instance.GetComponentInParent<PowerRelay>();
                }

                __instance.dockedObject.Recharge(__instance.powerRelay.GetPower(), 1f, out var amount);

                if (amount > 0.0f)
                {
                    __instance.chargingSound.Play();
                }
                else
                {
                    __instance.chargingSound.Stop();
                }
            }

            return false;
        }
    }
}

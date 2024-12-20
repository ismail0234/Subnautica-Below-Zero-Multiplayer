namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.Update))]
    public class SeaTruckDockingBay
    {
        private static void Prefix(global::SeaTruckDockingBay __instance)
        {
            if (Network.IsMultiplayerActive && __instance.dockedObject && __instance.dockedObject.transform.localPosition != Vector3.zero)
            {
                if (ZeroVector3.Distance(__instance.dockedObject.transform.position, __instance.transform.position) >= 25f)
                {
                    __instance.dockedObject.transform.localPosition = Vector3.zero;
                    __instance.dockedObject.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}

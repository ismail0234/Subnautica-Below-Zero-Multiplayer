namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch]
    public static class SeaTruckDocking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.OnTriggerEnter))]
        private static bool Prefix(global::SeaTruckDockingBay __instance, Collider other)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.dockedObject != null || __instance.timeUndocked + 2.0 >= Time.time)
            {
                return false;
            }

            var gameObject = UWE.Utils.GetEntityRoot(other.gameObject);
            if (gameObject == null)
            {
                gameObject = other.gameObject;
            }

            var lwe = other.GetComponentInParent<LargeWorldEntity>();
            if (lwe == null)
            {
                return false;
            }

            if (!gameObject.TryGetComponent<Dockable>(out var dockable) || ((IDockingBay)__instance).AllowedToDock(dockable) == false)
            {
                return false;
            }

            try
            {
                VehicleDockingEventArgs args = new VehicleDockingEventArgs(__instance.truckSegment.gameObject.GetIdentityId(), lwe.gameObject, TechType.SeaTruckDockingModule, Vector3.zero, Vector3.zero, Quaternion.identity);

                Handlers.Vehicle.OnDocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Docking.Prefix: {e}\n{e.StackTrace}");
            }
            
            return true;
        }
    }
}

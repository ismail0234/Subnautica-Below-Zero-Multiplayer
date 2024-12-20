namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class SeaTruckUndocking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.OnClickExosuit))]
        private static bool OnClickExosuit(global::SeaTruckDockingBay __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.dockedObject == null)
            {
                return false;
            }

            try
            {
                VehicleUndockingEventArgs args = new VehicleUndockingEventArgs(__instance.truckSegment.gameObject.GetIdentityId(), __instance.dockedObject.gameObject.GetIdentityId(), TechType.SeaTruckDockingModule, __instance.dockedObject.transform.position, __instance.dockedObject.transform.rotation, true);

                Handlers.Vehicle.OnUndocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckUndocking.OnClickExosuit: {e}\n{e.StackTrace}");
            }

            return true;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckDockingBay), nameof(global::SeaTruckDockingBay.OnClickEjectDocked))]
        private static bool OnClickEjectDocked(global::SeaTruckDockingBay __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.dockedObject == null)
            {
                return false;
            }

            try
            {
                VehicleUndockingEventArgs args = new VehicleUndockingEventArgs(__instance.truckSegment.gameObject.GetIdentityId(), __instance.dockedObject.gameObject.GetIdentityId(), TechType.SeaTruckDockingModule, __instance.dockedObject.transform.position, __instance.dockedObject.transform.rotation, false);

                Handlers.Vehicle.OnUndocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckUndocking.OnClickEjectDocked: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}

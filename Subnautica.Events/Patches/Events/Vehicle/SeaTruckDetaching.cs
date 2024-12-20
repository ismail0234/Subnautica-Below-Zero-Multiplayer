namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.OnClickDetachLever))]
    public static class SeaTruckDetaching
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SeaTruckSegment __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.rearConnection.GetConnection()?.truckSegment == null)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.gameObject)))
            {
                return false;
            }

            try
            {
                SeaTruckDetachingEventArgs args = new SeaTruckDetachingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                Handlers.Vehicle.OnSeaTruckDetaching(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckDetaching.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
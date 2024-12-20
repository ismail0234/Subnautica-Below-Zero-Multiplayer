namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class SeaTruckConnecting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckConnection), nameof(global::SeaTruckConnection.SetConnectedTo))]
        private static bool SeaTruckConnecting_SetConnectedTo(global::SeaTruckConnection __instance, SeaTruckConnection otherConnection)
        {
            if (!Network.IsMultiplayerActive || Tools.IsInStackTrace("OnKill"))
            {
                return true;
            }

            if (__instance.connection == otherConnection)
            {
                return false;
            }

            if (EventBlocker.IsEventBlocked(ProcessType.SeaTruckConnection))
            {
                return true;
            }

            if (!otherConnection)
            {
                if (__instance.connection)
                {
                    try
                    {
                        SeaTruckConnectingEventArgs args = new SeaTruckConnectingEventArgs(Network.Identifier.GetIdentityId(__instance.connection.truckSegment.gameObject, false), null, Network.Identifier.GetIdentityId(__instance.truckSegment.gameObject, false),  false, false);

                        Handlers.Vehicle.OnSeaTruckConnecting(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"SeaTruckConnecting.Prefix -> Disconnect: {e}\n{e.StackTrace}");
                    }
                }
            }
            else
            {
                try
                {
                    SeaTruckConnectingEventArgs args = new SeaTruckConnectingEventArgs(GetFrontModuleId(__instance, otherConnection), GetBackModuleId(__instance, otherConnection), Network.Identifier.GetIdentityId(otherConnection.truckSegment.GetFirstSegment().gameObject, false), true, false);

                    Handlers.Vehicle.OnSeaTruckConnecting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"SeaTruckConnecting.Prefix -> Connect: {e}\n{e.StackTrace}");
                }
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
        [HarmonyPatch(typeof(global::MoonpoolExpansionModuleConnection), nameof(global::MoonpoolExpansionModuleConnection.SetConnectedTo))]
        private static bool MoonpoolExpansionModuleConnection_SetConnectedTo(global::MoonpoolExpansionModuleConnection __instance, SeaTruckConnection otherConnection)
        {
            if (!Network.IsMultiplayerActive || Tools.IsInStackTrace("OnKill"))
            {
                return true;
            }

            if (__instance.connection == otherConnection)
            {
                return false;
            }

            if (EventBlocker.IsEventBlocked(ProcessType.SeaTruckConnection))
            {
                return true;
            }

            if (!otherConnection)
            {
                if (__instance.connection)
                {
                    try
                    {
                        SeaTruckConnectingEventArgs args = new SeaTruckConnectingEventArgs(__instance.GetComponentInParent<BaseDeconstructable>().gameObject.GetIdentityId(), __instance.connection.truckSegment.gameObject.GetIdentityId(), __instance.GetComponentInParent<MoonpoolExpansionManager>().dockedHead?.gameObject.GetIdentityId(), false, true);

                        Handlers.Vehicle.OnSeaTruckConnecting(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"SeaTruckConnecting.Prefix -> Disconnect: {e}\n{e.StackTrace}");
                    }
                }
            }
            else
            {

                try
                {
                    SeaTruckConnectingEventArgs args = new SeaTruckConnectingEventArgs(__instance.GetComponentInParent<BaseDeconstructable>().gameObject.GetIdentityId(), otherConnection.truckSegment.gameObject.GetIdentityId(), __instance.GetComponentInParent<MoonpoolExpansionManager>().dockedHead?.gameObject.GetIdentityId(), true, true);

                    Handlers.Vehicle.OnSeaTruckConnecting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"MoonpoolExpansionModuleConnection.Prefix -> Connect: {e}\n{e.StackTrace}");
                }
            }

            return false;
        }

        /**
         *
         * Ön modülü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetFrontModuleId(global::SeaTruckConnection __instance, SeaTruckConnection otherConnection)
        {
            if (__instance.connectionType == SeaTruckConnection.ConnectionType.Front)
            {
                return Network.Identifier.GetIdentityId(__instance.truckSegment.gameObject, false);
            }
             
            return Network.Identifier.GetIdentityId(otherConnection.truckSegment.gameObject, false);
        }

        /**
         *
         * Arka modülü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetBackModuleId(global::SeaTruckConnection __instance, SeaTruckConnection otherConnection)
        {
            if (__instance.connectionType == SeaTruckConnection.ConnectionType.Front)
            {
                return Network.Identifier.GetIdentityId(otherConnection.truckSegment.gameObject, false);
            }

            return Network.Identifier.GetIdentityId(__instance.truckSegment.gameObject, false);
        }
    }
}
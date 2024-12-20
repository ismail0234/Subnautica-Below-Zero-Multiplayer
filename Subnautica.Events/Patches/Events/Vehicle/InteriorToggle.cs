namespace Subnautica.Events.Patches.Events.Vehicle
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch]
    public class InteriorToggle
    {
        /*
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234 @hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SeaTruckSegment), nameof(SeaTruckSegment.EnterHatch))]
        private static void SeaTruckSegment_EnterHatch(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var expansionManager = __instance.GetFirstSegment()?.GetDockedMoonpoolExpansion();
                if (expansionManager == null)
                {
                    try
                    {
                        VehicleInteriorToggleEventArgs args = new VehicleInteriorToggleEventArgs(__instance.GetFirstSegment().gameObject.GetIdentityId(), true);

                        Handlers.Vehicle.OnInteriorToggle(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"SeaTruckSegment.EnterHatch: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        /*
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234 @hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.Exit))]
        private static void SeaTruckSegment_Exit(global::SeaTruckSegment __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    VehicleInteriorToggleEventArgs args = new VehicleInteriorToggleEventArgs(__instance.gameObject.GetIdentityId(), false);

                    Handlers.Vehicle.OnInteriorToggle(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SeaTruckSegment.Exit: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

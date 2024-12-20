namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::uGUI_ItemSelector), nameof(global::uGUI_ItemSelector.ResetSelector))]
    public class EnergyMixinClosed
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::uGUI_ItemSelector __instance)
        {
            if (Network.IsMultiplayerActive && __instance.manager != null)
            {
                var energyMixin = __instance.manager as EnergyMixin;
                if (energyMixin)
                {
                    var batterySlotId = EnergyMixinSelecting.GetBatterySlotId(energyMixin);
                    var vehicleId     = EnergyMixinSelecting.GetVehicleUniqueId(energyMixin);
                    var vehicleType   = EnergyMixinSelecting.GetVehicleType(energyMixin);

                    if (string.IsNullOrEmpty(batterySlotId) || string.IsNullOrEmpty(vehicleId) || !vehicleType.IsVehicle())
                    {
                        return;
                    }

                    try
                    {
                        EnergyMixinClosedEventArgs args = new EnergyMixinClosedEventArgs(vehicleId, batterySlotId, vehicleType);

                        Handlers.Player.OnEnergyMixinClosed(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"EnergyMixinClosed.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
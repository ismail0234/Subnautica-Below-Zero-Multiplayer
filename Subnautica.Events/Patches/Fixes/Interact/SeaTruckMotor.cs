namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.OnHoverSteeringWheel))]
    public class SeaTruckMotor
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SeaTruckMotor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(__instance.gameObject.GetIdentityId()))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            if (__instance.dockable && __instance.dockable.isDocked && __instance.dockable.bay != null)
            {
                var vehicleDockingBay = __instance.dockable.bay as global::VehicleDockingBay;
                if (vehicleDockingBay.MoonpoolExpansionEnabled())
                {
                    if (Network.HandTarget.IsBlocked(vehicleDockingBay.GetComponentInParent<BaseDeconstructable>().gameObject.GetIdentityId()))
                    {
                        Interact.ShowUseDenyMessage();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
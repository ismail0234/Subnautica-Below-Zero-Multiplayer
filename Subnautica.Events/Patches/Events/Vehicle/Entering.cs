namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class Entering
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::HoverbikeEnterTrigger), nameof(global::HoverbikeEnterTrigger.OnHandClick))]
        private static bool HoverbikeOnHandClick(global::HoverbikeEnterTrigger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.vehicleUpgradeConsoleInput.accessingConsole || __instance.hoverbike.GetPilotingCraft() || (!__instance.hoverbike.GetEnabled() || !__instance.hoverbike.AllowedToPilot()))
            {
                return false;
            }

            try
            {
                VehicleEnteringEventArgs args = new VehicleEnteringEventArgs(__instance.hoverbike.gameObject.GetIdentityId(), TechType.Hoverbike);

                Handlers.Vehicle.OnEntering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverbikeEnterTrigger.OnHandClick: {e}\n{e.StackTrace}");
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
        [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.OnHandClick))]
        private static bool ExosuitOnHandClick(global::Vehicle __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.GetPilotingMode() || !__instance.GetEnabled())
            {
                return false;
            }

            if (!__instance.TryGetComponent<global::Exosuit>(out var temp))
            {
                return false;
            }

            try
            {
                VehicleEnteringEventArgs args = new VehicleEnteringEventArgs(__instance.gameObject.GetIdentityId(), TechType.Exosuit);

                Handlers.Vehicle.OnEntering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Vehicle.OnHandClick: {e}\n{e.StackTrace}");
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
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.OnClickSteeringWheel))]
        private static bool SeaTruckOnClickSteeringWheel(global::SeaTruckMotor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.AllowedToUseSteeringWheel())
            {
                return false;
            }

            if (__instance.dockable && __instance.dockable.isDocked && __instance.dockable.bay != null)
            {
                var vehicleDockingBay = __instance.dockable.bay as VehicleDockingBay;
                if (vehicleDockingBay.MoonpoolExpansionEnabled())
                {
                    try
                    {
                        VehicleUndockingEventArgs args = new VehicleUndockingEventArgs(vehicleDockingBay.GetComponentInParent<BaseDeconstructable>().gameObject.GetIdentityId(), __instance.gameObject.GetIdentityId(), TechType.BaseMoonpoolExpansion, vehicleDockingBay.GetDockedObject().transform.position, vehicleDockingBay.GetDockedObject().transform.rotation, true);

                        Handlers.Vehicle.OnUndocking(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Entering.SeaTruckOnClickSteeringWheel: {e}\n{e.StackTrace}");
                    }

                    return true;
                }
            }

            try
            {
                VehicleEnteringEventArgs args = new VehicleEnteringEventArgs(__instance.gameObject.GetIdentityId(), CraftData.GetTechType(__instance.gameObject));

                Handlers.Vehicle.OnEntering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckMotor.OnClickSteeringWheel: {e}\n{e.StackTrace}");
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
        [HarmonyPatch(typeof(global::SpyPenguinRemoteManager), nameof(global::SpyPenguinRemoteManager.TryActivatePenginFromDistance))]
        private static bool TryActivatePenginFromDistance(global::SpyPenguinRemoteManager __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.registeredPenguins.Count == 0 || global::Player.main.cinematicModeActive || !global::Player.main.IsUnderwater() && global::Player.main.groundMotor.enabled && !global::Player.main.groundMotor.IsGrounded())
            {
                return false;
            }

            try
            {
                VehicleEnteringEventArgs args = new VehicleEnteringEventArgs(null, TechType.SpyPenguin);

                Handlers.Vehicle.OnEntering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SpyPenguinRemoteManager.TryActivatePenginFromDistance: {e}\n{e.StackTrace}");
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
        [HarmonyPatch(typeof(global::MapRoomScreen), nameof(global::MapRoomScreen.OnHandClick))]
        private static bool MapRoomScreenOnHandClick(global::MapRoomScreen __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (MapRoomCamera.cameras.Count <= 0)
            {
                return false;
            }

            try
            {
                VehicleEnteringEventArgs args = new VehicleEnteringEventArgs(__instance.mapRoomFunctionality.GetBaseDeconstructable().gameObject.GetIdentityId(), TechType.MapRoomCamera);

                Handlers.Vehicle.OnEntering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MapRoomScreen.OnHandClick: {e}\n{e.StackTrace}");
            }
      
            return true;
        }
    }
}
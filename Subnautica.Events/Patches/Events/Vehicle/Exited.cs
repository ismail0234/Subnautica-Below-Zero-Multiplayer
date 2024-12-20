namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class Exited
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Hoverbike), nameof(global::Hoverbike.ExitVehicle))]
        private static void Hoverbike_ExitVehicle(global::Hoverbike __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), TechType.Hoverbike);

                    Handlers.Vehicle.OnExited(args);
                }
                catch (Exception e)
                {
                    Log.Error($"Hoverbike.ExitVehicle: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.OnPilotModeEnd))]
        private static void Vehicle_OnPilotModeEnd(global::Hoverbike __instance)
        {
            if (Network.IsMultiplayerActive && __instance.TryGetComponent<global::Exosuit>(out var temp))
            {
                try
                {
                    VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), TechType.Exosuit);

                    Handlers.Vehicle.OnExited(args);
                }
                catch (Exception e)
                {
                    Log.Error($"Vehicle.OnPilotModeEnd: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.StopPiloting))]
        private static void SeaTruckMotor_StopPiloting(global::SeaTruckMotor __instance)
        {
            if (Network.IsMultiplayerActive && __instance.piloting)
            {
                try
                {
                    VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), CraftData.GetTechType(__instance.gameObject));

                    Handlers.Vehicle.OnExited(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SeaTruckMotor.StopPiloting: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckAnimation), nameof(global::SeaTruckAnimation.TriggerAnimation))]
        private static void SeaTruckAnimation_TriggerAnimation(global::SeaTruckAnimation __instance, SeaTruckAnimation.Animation anim)
        {
            if (Network.IsMultiplayerActive && (anim == SeaTruckAnimation.Animation.ExitPilot || anim == SeaTruckAnimation.Animation.EndPilot))
            {
                var controller = __instance.GetController(anim);
                if (controller)
                {
                    try
                    {
                        VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), CraftData.GetTechType(__instance.gameObject));

                        Handlers.Vehicle.OnExited(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"SeaTruckAnimation.TriggerAnimation: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.DisablePenguinCam))]
        private static void SpyPenguin_DisablePenguinCam(global::SpyPenguin __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), TechType.SpyPenguin);

                    Handlers.Vehicle.OnExited(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SpyPenguin.DisablePenguinCam: {e}\n{e.StackTrace}");
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.FreeCamera))]
        private static void MapRoomCamera_FreeCamera(global::MapRoomCamera __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    VehicleExitedEventArgs args = new VehicleExitedEventArgs(__instance.gameObject.GetIdentityId(), TechType.MapRoomCamera);

                    Handlers.Vehicle.OnExited(args);
                }
                catch (Exception e)
                {
                    Log.Error($"MapRoomCamera.FreeCamera: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
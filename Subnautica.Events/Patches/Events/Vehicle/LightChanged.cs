namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class LightChanged
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ToggleLights), nameof(global::ToggleLights.SetLightsActive))]
        private static void ToggleLights_SetLightsActive(global::ToggleLights __instance, bool isActive)
        {
            if (!Network.IsMultiplayerActive)
            {
                return;
            }

            if (!__instance.IsToggleEnabled || isActive == __instance.lightsActive)
            {
                return;
            }

            var hoverbike = __instance.GetComponentInParent<global::Hoverbike>();
            if (hoverbike == null)
            {
                return;
            }

            try
            {
                LightChangedEventArgs args = new LightChangedEventArgs(hoverbike.gameObject.GetIdentityId(), isActive, TechType.Hoverbike);

                Handlers.Vehicle.OnLightChanged(args);
            }
            catch (Exception e)
            {
                Log.Error($"LightChanged.ToggleLights_SetLightsActive: {e}\n{e.StackTrace}");
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckLights), nameof(global::SeaTruckLights.Update))]
        private static void SeaTruckLights_Update(global::SeaTruckLights __instance)
        {
            if (Network.IsMultiplayerActive && __instance.motor && __instance.floodLight)
            {
                if (__instance.motor.IsPiloted() && global::Player.main.GetRightHandDown())
                {
                    try
                    {
                        LightChangedEventArgs args = new LightChangedEventArgs(__instance.gameObject.GetIdentityId(), !__instance.lightsActive, TechType.SeaTruck);

                        Handlers.Vehicle.OnLightChanged(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"LightChanged.SeaTruckLights_Update: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomCamera), nameof(global::MapRoomCamera.Update))]
        private static void MapRoomCamera_Update(global::MapRoomCamera __instance)
        {
            if (Network.IsMultiplayerActive && __instance.IsControlled() && __instance.inputStackDummy.activeInHierarchy)
            {
                if (GameInput.GetButtonDown(GameInput.Button.RightHand))
                {
                    try
                    {
                        LightChangedEventArgs args = new LightChangedEventArgs(__instance.gameObject.GetIdentityId(), !__instance.lightsParent.activeInHierarchy, TechType.MapRoomCamera);

                        Handlers.Vehicle.OnLightChanged(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"LightChanged.MapRoomCamera_Update: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
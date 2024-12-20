namespace Subnautica.Events.Patches.Fixes.Game
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class VehicleEnergyConsumer
    {  
        [HarmonyPrefix]            
        [HarmonyPatch(typeof(global::Hoverbike), nameof(global::Hoverbike.UpdateEnergy))]
        private static bool HoverbikeUpdateEnergy()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.ScheduledUpdate))]
        private static bool ExosuitScheduledUpdate()
        {
            return !Network.IsMultiplayerActive;
        }
    
        [HarmonyPrefix]        
        [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.ConsumeEngineEnergy))]
        private static bool VehicleConsumeEngineEnergy()
        {
            return !Network.IsMultiplayerActive;
        }
    
        [HarmonyPrefix]        
        [HarmonyPatch(typeof(global::Hoverbike), nameof(global::Hoverbike.HealAndCharge))]
        private static bool HoverbikeHealAndCharge()
        {
            return !Network.IsMultiplayerActive;
        }
    
        [HarmonyPrefix]        
        [HarmonyPatch(typeof(global::BaseRechargePlatform), nameof(global::BaseRechargePlatform.Update))]
        private static bool BaseRechargePlatformUpdate()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.ConsumeEnergy), new Type[] { typeof(float) })]
        private static bool VehicleConsumeEnergy(global::Vehicle __instance, float amount)
        {
            return !Network.IsMultiplayerActive ? true : __instance.HasEnoughEnergy(amount);
        }
    }
}
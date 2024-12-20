namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class EnergyConsumer
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::PowerSource), nameof(global::PowerSource.ModifyPower))]
        public static bool PowerSourceModifyPower(ref bool __result, float amount, out float modified)
        {
            modified = 0.0f;

            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = false;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.ConsumePower))]
        private static bool TechLightConsumePower(ref bool __result)
        {
            if (Network.IsMultiplayerActive)
            {
                __result = true;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.UpdatePower))]
        public static bool JukeboxUpdatePower()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseSpotLight), nameof(global::BaseSpotLight.UpdatePower))]
        private static bool BaseSpotLightUpdatePower()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::TechLight), nameof(global::TechLight.UpdatePower))]
        private static bool TechLightUpdatePower()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

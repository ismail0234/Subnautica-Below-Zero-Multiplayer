namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class BaseBioReactorProduction
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseBioReactor), nameof(global::BaseBioReactor.Update))]
        public static bool BaseBioReactor()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BaseNuclearReactor), nameof(global::BaseNuclearReactor.Update))]
        public static bool BaseNuclearReactor()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SolarPanel), nameof(global::SolarPanel.Update))]
        public static bool SolarPanel()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ThermalPlant), nameof(global::ThermalPlant.AddPower))]
        public static bool ThermalPlant()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Dockable), nameof(global::Dockable.AddEnergy))]
        public static bool Dockable()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

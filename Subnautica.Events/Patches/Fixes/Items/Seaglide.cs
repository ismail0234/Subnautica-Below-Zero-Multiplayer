namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Seaglide
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::EnergyEffect), nameof(global::EnergyEffect.Update))]
        private static bool SeaglideEnergyEffectUpdate(global::EnergyEffect __instance)
        {
            if (Network.IsMultiplayerActive && __instance.energy == null)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Seaglide), nameof(global::Seaglide.Update))]
        private static bool SeaglideUpdate(global::Seaglide __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.usingPlayer == global::Player.main)
                {
                    return true;
                }

                __instance.screenEffectModel.SetActive(false);
                return false;
            }

            return true;
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public static class FiltrationMachineTryFilter
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::FiltrationMachine), nameof(global::FiltrationMachine.TryFilterWater))]
        private static bool TryFilterWater()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::FiltrationMachine), nameof(global::FiltrationMachine.TryFilterSalt))]
        private static bool TryFilterSalt()
        {
            return !Network.IsMultiplayerActive;
        }
    }

    [HarmonyPatch(typeof(global::FiltrationMachine), nameof(global::FiltrationMachine.UpdateFiltering))]
    public static class FiltrationMachineUpdateFiltering
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::FiltrationMachine __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance._constructed < 1.0f)
                {
                    return false;
                }

                var model = __instance.GetModel();
                if (model != null)
                {
                    model.SetDirty();
                }

                return false;
            }

            return true;
        }
    }
}
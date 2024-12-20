namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.EventSystems;

    using UWE;

    [HarmonyPatch(typeof(global::FruitPlant), nameof(global::FruitPlant.OnGrown))]
    public static class PlanterGrowned
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::FruitPlant __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    PlanterGrownedEventArgs args = new PlanterGrownedEventArgs(__instance);

                    Handlers.Furnitures.OnPlanterGrowned(args);
                }
                catch (Exception e)
                {
                    Log.Error($"PlanterGrowned.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
namespace Subnautica.Events.Patches.Fixes.Game
{
    using System;
    using System.Collections;

    using HarmonyLib;
    
    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class WatermarkVersion
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(uGUI_BuildWatermark), nameof(uGUI_BuildWatermark.Start))]
        private static IEnumerator Start(IEnumerator values, uGUI_BuildWatermark __instance)
        {
            __instance.UpdateText();
            global::Language.OnLanguageChanged += new Action(__instance.OnLanguageChanged);

            while (!LightmappedPrefabs.main || LightmappedPrefabs.main.IsWaitingOnLoads() || !uGUI.main || !uGUI.main.loading || uGUI.main.loading.IsLoading || !PAXTerrainController.main || PAXTerrainController.main.isWorking)
            {
                yield return null;
            }

            if (Network.IsMultiplayerActive)
            {
                __instance.text.text = Settings.GetWatermarkText();    
            }
            else
            {

                __instance.gameObject.SetActive(false);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(uGUI_BuildWatermark), nameof(uGUI_BuildWatermark.UpdateText))]
        private static bool UpdateText(uGUI_BuildWatermark __instance)
        {
            __instance.text.text = global::Language.main.GetFormat("EarlyAccessWatermarkFormat", SNUtils.GetDateTimeOfBuild(), SNUtils.GetPlasticChangeSetOfBuild()) + "\n" + Settings.GetWatermarkText();
            return false;
        }
    }
}
namespace Subnautica.Events.Patches.Fixes.Game
{
    using Subnautica.API.Features;

    using HarmonyLib;

    using System.Collections.Generic;

    [HarmonyPatch]
    public static class LoadingStage
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::LoadingStage), nameof(global::LoadingStage.FillStages))]
        private static bool FillStages(Dictionary<string, float> stages)
        {
            foreach (string key in global::LoadingStage.common)
            {
                if (!stages.ContainsKey(key))
                {
                    stages[key] = 0.0f;
                }
            }

            if (global::Utils.GetContinueMode() && !stages.ContainsKey("WorldSettle"))
            {
                stages["WorldSettle"] = 0.0f;
            }

            if (GameModeManager.HaveGameOptionsSet && GameModeManager.GetOption<bool>(GameOption.InitialEquipmentPack) && !stages.ContainsKey("Equipment"))
            {
                stages["Equipment"] = 0f;
            }

            return false;
        }
    }
}

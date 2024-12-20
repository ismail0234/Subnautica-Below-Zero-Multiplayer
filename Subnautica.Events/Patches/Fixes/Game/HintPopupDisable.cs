namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class HintPopupDisable
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(uGUI_FeedbackCollector), nameof(uGUI_FeedbackCollector.ShowReplyNotification))]
        private static bool ShowReplyNotification()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(uGUI_FeedbackCollector), nameof(uGUI_FeedbackCollector.HintShow))]
        private static bool HintShow()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(uGUI_FeedbackCollector), nameof(uGUI_FeedbackCollector.HintHide))]
        private static bool HintHide()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

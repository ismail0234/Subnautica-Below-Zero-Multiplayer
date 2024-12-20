namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    [HarmonyPatch(typeof(EndCreditsManager), nameof(EndCreditsManager.Start))]
    public static class Credits
    {
        private static void Postfix(EndCreditsManager __instance)
        {
            __instance.startFadeLogoTime -= 4f;
            __instance.centerText.text    = string.Format("{0}\n{1}", API.Features.Settings.GetCreditsText(), __instance.centerText.text);
        }
    }
}

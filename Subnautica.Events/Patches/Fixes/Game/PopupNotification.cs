namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class PopupNotification
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::uGUI_PopupNotification), nameof(global::uGUI_PopupNotification.Show))]
        private static void Show(global::uGUI_PopupNotification __instance)
        {
            if (Network.IsMultiplayerActive && __instance.current.sound != null)
            {
                if (__instance.current.sound.name.Contains("new_blueprint") || __instance.current.sound.name.Contains("new_tech"))
                {
                    __instance.StopSound(true);
                    __instance.StartSound(__instance.current.sound);
                }
            }
        }
    }
}

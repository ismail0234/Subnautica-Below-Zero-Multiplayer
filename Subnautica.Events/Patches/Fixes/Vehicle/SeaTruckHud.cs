namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::uGUI_SeaTruckHUD), nameof(global::uGUI_SeaTruckHUD.Update))]
    public class SeaTruckHud
    {
        private static bool Prefix(global::uGUI_SeaTruckHUD __instance)
        {
            if (Network.IsMultiplayerActive && global::Player.main != null && !global::Player.main.inSeatruckPilotingChair)
            {
                __instance.root.SetActive(false);
                return false;
            }

            return true;
        }
    }
}

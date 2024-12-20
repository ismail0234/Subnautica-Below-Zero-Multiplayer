namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(MainGameController), nameof(MainGameController.SetInitialPlayerPosition))]
    public class WorldLoadingBefore
    {
        private static bool Prefix()
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            return false;
        }
    }
}

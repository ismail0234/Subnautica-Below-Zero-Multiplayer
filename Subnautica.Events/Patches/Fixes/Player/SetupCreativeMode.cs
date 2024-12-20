namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.AddInitialEquipment))]
    public class SetupCreativeMode
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
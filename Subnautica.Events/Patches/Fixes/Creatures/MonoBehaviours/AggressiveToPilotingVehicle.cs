namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch(typeof(global::AggressiveToPilotingVehicle), nameof(global::AggressiveToPilotingVehicle.UpdateAggression))]
    public class AggressiveToPilotingVehicle
    {
        public static bool Prefix(global::AggressiveToPilotingVehicle __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.CancelInvoke("UpdateAggression");
            return false;
        }
    }
}

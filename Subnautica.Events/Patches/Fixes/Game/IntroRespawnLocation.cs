namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Story.StoryGoalManager), nameof(global::Story.StoryGoalManager.IsGoalComplete))]
    public class IntroRespawnLocation
    {
        private static bool Prefix(string key)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return key != "IntroRespawnLocation";
        }
    }
}

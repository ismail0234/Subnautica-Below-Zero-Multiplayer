namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch]
    public class BirdBehaviour
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BirdBehaviour), nameof(global::BirdBehaviour.InitializeOnce))]
        public static bool InitializeOnce()
        {
            return !Network.IsMultiplayerActive;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BirdBehaviour), nameof(global::BirdBehaviour.InitializeAgain))]
        public static bool InitializeAgain()
        {
            return !Network.IsMultiplayerActive;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BirdBehaviour), nameof(global::BirdBehaviour.Update))]
        public static bool Update(global::BirdBehaviour __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.AllowCreatureUpdates(!__instance.drowning);
            return false;
        }
    }
}

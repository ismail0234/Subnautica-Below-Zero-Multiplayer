namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Jellyfish), nameof(global::Jellyfish.Update))]
    public class Jellyfish
    {
        private static bool Prefix(global::Jellyfish __instance)
        {
            return !Network.IsMultiplayerActive;
        }
    }
}

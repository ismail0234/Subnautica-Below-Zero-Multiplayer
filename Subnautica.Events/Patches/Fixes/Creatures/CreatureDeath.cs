namespace Subnautica.Events.Patches.Fixes.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch]
    public class CreatureDeath
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CreatureDeath), nameof(global::CreatureDeath.SpawnRespawner))]
        private static bool SpawnRespawner(global::CreatureDeath __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return !__instance.gameObject.GetTechType().IsSynchronizedCreature();
        }
    }
}

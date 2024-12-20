namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using Subnautica.API.Features;

    using HarmonyLib;

    [HarmonyPatch(typeof(global::CreatureAggressionManager), nameof(global::CreatureAggressionManager.OnMeleeAttack))]
    public class CreatureAggressionManager
    {
        public static bool Prefix(global::CreatureAggressionManager __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return __instance.enabled;
        }
    }
}

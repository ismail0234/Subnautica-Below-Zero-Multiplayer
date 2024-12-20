namespace Subnautica.Events.Patches.Fixes.Creatures
{
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using HarmonyLib;

    [HarmonyPatch(typeof(global::Creature), nameof(global::Creature.InitializeOnce))]
    public class Creature
    {
        public static bool Prefix(global::Creature __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return !__instance.IsSynchronized();
        }
    }
}

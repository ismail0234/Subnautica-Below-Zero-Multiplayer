namespace Subnautica.Events.Patches.Fixes.Creatures.Actions
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Roost), nameof(global::Roost.GetRoostSpot))]
    public class Roost
    {
        private static bool Prefix(global::Roost __instance, ref Vector3 __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __result = __instance.creature.leashPosition;
            return false;
        }
    }
}

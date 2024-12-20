namespace Subnautica.Events.Patches.Fixes.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(PingInstance), nameof(PingInstance.GetPosition))]
    public static class PlayerSignalPosition
    {
        private static bool Prefix(PingInstance __instance, ref Vector3 __result)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.origin.name.Contains("MultiplayerPlayerSignal"))
            {
                var position = __instance.origin.position;
                position.y += .5f;

                __result = position;
                return false;
            }

            return true;
        }
    }
}

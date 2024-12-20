namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.PlayerIsInRange))]
    public static class CrafterAutoClose
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::GhostCrafter __instance, ref bool __result, float distance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var maxDistance = distance * distance;

            if (ZeroVector3.Distance(global::Player.main.transform.position, __instance.transform.position) < maxDistance)
            {
                __result = true;
                return false;
            }

            foreach (var player in ZeroPlayer.GetPlayers())
            {
                if (ZeroVector3.Distance(player.Position, __instance.transform.position) < maxDistance)
                {
                    __result = true;
                    return false;
                }
            }

            __result = false;
            return false;
        }
    }
}

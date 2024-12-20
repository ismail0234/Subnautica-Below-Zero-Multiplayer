namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::BaseControlRoom), nameof(global::BaseControlRoom.CacheGeometry))]
    public static class BaseControlRoom
    {
        private static void Postfix(global::BaseControlRoom __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.ClearCache();
            }
        }
    }
}

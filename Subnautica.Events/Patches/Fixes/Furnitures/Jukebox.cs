namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Jukebox
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Jukebox), nameof(global::Jukebox.HandleLooping))]
        private static bool JukeboxHandleLooping(global::Jukebox __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance._file = null;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Jukebox), nameof(global::Jukebox.HandleOpenError))]
        private static bool JukeboxHandleOpenError(global::Jukebox __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance._file = null;
            return false;
        }
    }
}

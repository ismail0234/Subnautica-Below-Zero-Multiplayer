namespace Subnautica.Events.Patches.Identity.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::UniqueIdentifier), nameof(global::UniqueIdentifier.EnsureGuid))]
    public static class UniqueIdentifier
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(ref string __result, string guid)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (string.IsNullOrEmpty(guid))
            {
                __result = Network.Identifier.GenerateUniqueId();
            }
            else
            {
                __result = guid;    
            }

            return false;
        }
    }
}
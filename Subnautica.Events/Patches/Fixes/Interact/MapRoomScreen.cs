namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::MapRoomScreen), nameof(global::MapRoomScreen.OnHandHover))]
    public class MapRoomScreen
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::MapRoomScreen __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(__instance.mapRoomFunctionality.GetBaseDeconstructable()?.gameObject.GetIdentityId()))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::BaseControlRoom), nameof(global::BaseControlRoom.OnHoverMinimapConsole))]
    public class BaseControlRoom
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseControlRoom __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Interact.IsBlocked(TechGroup.GetBaseControlRoomNavigateId(Network.Identifier.GetIdentityId(__instance.gameObject, false))))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

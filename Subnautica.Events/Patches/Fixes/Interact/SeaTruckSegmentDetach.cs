namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.OnHoverDetachLever))]
    public class SeaTruckSegmentDetach
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SeaTruckSegment __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::DockedVehicleHandTarget), nameof(global::DockedVehicleHandTarget.OnHandHover))]
    public class DockedVehicleHandTarget
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::DockedVehicleHandTarget __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (IsBlocked(__instance))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        /**
         *
         * Bloklanma durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsBlocked(global::DockedVehicleHandTarget __instance)
        {
            if (__instance.dockingBay.GetDockedObject() != null)
            {
                if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.dockingBay.GetDockedObject().gameObject, false)))
                {
                    return true;
                }
            }

            var baseDeconstructable = __instance.gameObject.GetComponentInParent<BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return Interact.IsBlocked(Network.Identifier.GetIdentityId(baseDeconstructable.gameObject, false));
            }

            return false;
        }
    }
}

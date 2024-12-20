namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::VehicleUpgradeConsoleInput), nameof(global::VehicleUpgradeConsoleInput.OnHandHover))]
    public class VehicleUpgradeConsoleInput
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::VehicleUpgradeConsoleInput __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.equipment == null)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.GetComponentInParent<LargeWorldEntity>().gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::EnergyMixin), nameof(global::EnergyMixin.HandHover))]
    public class EnergyMixin
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::EnergyMixin __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.allowBatteryReplacement)
            {
                return false;
            }

            var uniqueId = Network.Identifier.GetIdentityId(__instance.storageRoot.gameObject, false);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return true;
            }

            if (Interact.IsBlocked(uniqueId) || Interact.IsBlocked(ZeroGame.GetVehicleBatteryLabelUniqueId(uniqueId)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
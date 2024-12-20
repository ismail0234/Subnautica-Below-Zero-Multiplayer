namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Charger), nameof(global::Charger.OnHandHover))]
    public class BatteryCharger
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Charger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            Constructable component = __instance.gameObject.GetComponent<Constructable>();
            if (component && !component.constructed)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(component.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}

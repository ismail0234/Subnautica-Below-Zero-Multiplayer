namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Vehicle), nameof(global::Vehicle.OnHandHover))]
    public class Vehicle
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Vehicle __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.GetPilotingMode() || !__instance.GetEnabled())
            {
                return false;
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

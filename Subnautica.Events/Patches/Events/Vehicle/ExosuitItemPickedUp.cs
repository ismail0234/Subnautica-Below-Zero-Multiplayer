namespace Subnautica.Events.Patches.Events.Vehicle
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::ExosuitClawArm), nameof(global::ExosuitClawArm.OnPickup))]
    public class ExosuitItemPickedUp
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ExosuitClawArm __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var exosuit = __instance.GetComponentInParent<Exosuit>();
            var pickup  = exosuit?.GetActiveTarget()?.GetComponent<Pickupable>();
            if (pickup == null)
            {
                return false;
            }

            try
            {

                ExosuitItemPickedUpEventArgs args = new ExosuitItemPickedUpEventArgs(Network.Identifier.GetIdentityId(exosuit.gameObject), Network.Identifier.GetIdentityId(pickup.gameObject), pickup);

                Handlers.Vehicle.OnExosuitItemPickedUp(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ExosuitItemPickedUp.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
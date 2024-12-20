namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.TryPickup))]
    public class SpyPenguinItemPickedUp
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SpyPenguin __instance, Pickupable pickup)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (pickup == null || !pickup.isPickupable || !__instance.inventory.container.HasRoomFor(pickup))
            {
                return false;
            }

            try
            {
                SpyPenguinItemPickedUpEventArgs args = new SpyPenguinItemPickedUpEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), Network.Identifier.GetIdentityId(pickup.gameObject), pickup);

                Handlers.Items.OnSpyPenguinItemPickedUp(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SpyPenguinItemPickedUp.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.Pickup))]
    public static class ItemPickedUp
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Inventory __instance, ref bool __result, global::Pickupable pickupable)
        {
            if (Network.IsMultiplayerActive && __instance._container.HasRoomFor(pickupable) && !EventBlocker.IsEventBlocked(ProcessType.ItemPickup))
            {
                try
                {
                    PlayerItemPickedUpEventArgs args = new PlayerItemPickedUpEventArgs(Network.Identifier.GetIdentityId(pickupable.gameObject, false), pickupable.GetTechType(), pickupable);

                    Handlers.Player.OnItemPickedUp(args);

                    if (args.IsAllowed)
                    {
                        return true;
                    }

                    __result = true;
                    return false;
                }
                catch (Exception e)
                {
                    Log.Error($"ItemPickedUp.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}
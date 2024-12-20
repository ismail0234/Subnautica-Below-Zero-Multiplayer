namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.OnRemoveItem))]
    public class ItemRemoved
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Inventory __instance, InventoryItem item)
        {
            if (Network.IsMultiplayerActive && !__instance.isTerminating)
            {
                try
                {
                    InventoryItemRemovedEventArgs args = new InventoryItemRemovedEventArgs(Network.Identifier.GetIdentityId(item.item.gameObject, false));

                    Handlers.Inventory.OnItemRemoved(args);
                }
                catch (Exception e)
                {
                    Log.Error($"InventoryItemRemoved.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
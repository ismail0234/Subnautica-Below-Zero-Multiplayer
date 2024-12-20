namespace Subnautica.Events.Patches.Events.Inventory
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.OnAddItem))]
    public class ItemAdded
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(InventoryItem item)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    InventoryItemAddedEventArgs args = new InventoryItemAddedEventArgs(Network.Identifier.GetIdentityId(item.item.gameObject, false), item.item);

                    Handlers.Inventory.OnItemAdded(args);
                }
                catch (Exception e)
                {
                    Log.Error($"InventoryItemAdded.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using System;

    [HarmonyPatch(typeof(global::QuickSlots), nameof(global::QuickSlots.Bind))]
    public class QuickSlotBinded
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::QuickSlots __instance, int slotID, InventoryItem item)
        {
            if(Network.IsMultiplayerActive)
            {
                if (slotID < 0 || slotID >= __instance.slotCount || item == null || !item.isBindable)
                {
                    return;
                }
                    
                try
                {
                   Handlers.Inventory.OnQuickSlotBinded();
                }
                catch (Exception e)
                {
                    Log.Error($"QuickSlotBinded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
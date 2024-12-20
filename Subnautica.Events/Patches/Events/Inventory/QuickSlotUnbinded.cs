namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using System;

    [HarmonyPatch(typeof(global::QuickSlots), nameof(global::QuickSlots.Unbind))]
    public class QuickSlotUnbinded
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::QuickSlots __instance, int slotID)
        {
            if(Network.IsMultiplayerActive)
            {
                if (slotID < 0 || slotID >= __instance.slotCount || __instance.binding[slotID] == null)
                {
                    return;
                }

                try
                {
                    Handlers.Inventory.OnQuickSlotUnbinded();
                }
                catch (Exception e)
                {
                    Log.Error($"QuickSlotUnbinded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
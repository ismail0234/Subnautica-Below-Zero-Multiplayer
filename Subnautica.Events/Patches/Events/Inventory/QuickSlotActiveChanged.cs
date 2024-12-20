namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(global::QuickSlots), nameof(global::QuickSlots.NotifySelect))]
    public class QuickSlotActiveChanged
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(int slotID)
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    QuickSlotActiveChangedEventArgs args = new QuickSlotActiveChangedEventArgs(slotID);

                    Handlers.Inventory.OnQuickSlotActiveChanged(args);
                }
                catch (Exception e)
                {
                    Log.Error($"QuickSlotActiveChanged.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
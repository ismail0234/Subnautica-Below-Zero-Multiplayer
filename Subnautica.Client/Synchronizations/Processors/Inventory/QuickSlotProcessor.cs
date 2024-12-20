namespace Subnautica.Client.Synchronizations.Processors.Inventory
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    using System.Collections;
    
    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class QuickSlotProcessor : NormalProcessor
    {
        /**
         *
         * Zamanlanmış veri gönderim durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsSending { get; set; } = false;

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            return true;
        }

        /**
         *
         * Oyuncu bir eşyayı slotlara atadığında tetiklenir.
         * Oyuncu bir eşyayı slotlardan kaldırdığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnProcessQuickSlot()
        {
            if (!IsSending && !EventBlocker.IsEventBlocked(ProcessType.InventoryQuickSlot))
            {
                UWE.CoroutineHost.StartCoroutine(SendServerData());
            }
        }

        /**
         *
         * Zamanlanmış veriyi sunucuya gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator SendServerData()
        {
            IsSending = true;

            yield return new WaitForSecondsRealtime(1f);

            ServerModel.InventoryQuickSlotItemArgs result = new ServerModel.InventoryQuickSlotItemArgs()
            {
                Slots      = global::Inventory.main.quickSlots.SaveBinding(),
                ActiveSlot = global::Inventory.main.quickSlots.activeSlot,
            };

            NetworkClient.SendPacket(result);
            
            IsSending = false;
        }
    }
}

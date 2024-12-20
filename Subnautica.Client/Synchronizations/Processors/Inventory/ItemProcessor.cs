namespace Subnautica.Client.Synchronizations.Processors.Inventory
{
    using System.Collections;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;

    using UnityEngine;

    using UWE;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ItemProcessor : NormalProcessor
    {
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
         * Oyuncu'nun envanterine bir eşya geldiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInventoryItemAdded(InventoryItemAddedEventArgs ev)
        {
            if (!EventBlocker.IsEventBlocked(ProcessType.InventoryItem))
            {
                CoroutineHost.StartCoroutine(ItemProcessor.SendPacketToServer(ev.UniqueId, ev.Item, true));
            }
        }

        /**
         *
         * Oyuncu'nun envanterinden bir eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInventoryItemRemoved(InventoryItemRemovedEventArgs ev)
        {
            if (!EventBlocker.IsEventBlocked(ProcessType.InventoryItem))
            {
                CoroutineHost.StartCoroutine(ItemProcessor.SendPacketToServer(ev.UniqueId, isAdded: false));
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator SendPacketToServer(string uniqueId, Pickupable item = null, bool isAdded = false)
        {
            if (isAdded)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            ServerModel.InventoryItemArgs request = new ServerModel.InventoryItemArgs()
            {
                ItemId  = uniqueId,
                Item    = item != null ? StorageItem.Create(item) : null,
                IsAdded = isAdded,
            };

            NetworkClient.SendPacket(request);
        }
    }
}

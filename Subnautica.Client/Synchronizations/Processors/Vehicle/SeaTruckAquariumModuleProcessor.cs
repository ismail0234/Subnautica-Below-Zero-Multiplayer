namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SeaTruckAquariumModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckAquariumModuleArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (packet.IsAdded)
            {
                Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), packet.WorldPickupItem);
            }
            else
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), packet.WorldPickupItem);
            }

            return true;
        }

        /**
         *
         * Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemAdding(StorageItemAddingEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckAquariumModule)
            {
                ev.IsAllowed = false;

                SeaTruckAquariumModuleProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
            }
        }

        /**
         *
         * Depolama'dan eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemRemoving(StorageItemRemovingEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckAquariumModule)
            {
                ev.IsAllowed = false;

                SeaTruckAquariumModuleProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem = null, bool isAdded = false)
        {
            ServerModel.SeaTruckAquariumModuleArgs request = new ServerModel.SeaTruckAquariumModuleArgs()
            {
                UniqueId        = uniqueId,
                IsAdded         = isAdded,
                WorldPickupItem = pickupItem,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
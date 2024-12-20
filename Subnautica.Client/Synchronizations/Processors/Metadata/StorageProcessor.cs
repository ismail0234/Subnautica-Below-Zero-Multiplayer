namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class StorageProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var storageContainer = Network.Identifier.GetComponentByGameObject<global::StorageContainer>(packet.UniqueId);
            if (storageContainer == null)
            {
                return false;
            }

            if (isSilence)
            {
                this.InitializeStorage(techType, packet.UniqueId, packet.Component, storageContainer.container);
                return true;          
            }

            var component = packet.Component.GetComponent<Metadata.StorageLocker>();
            if (component == null)
            {
                return false;
            }

            if (component.IsAdded)
            {
                Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), component.WorldPickupItem);
            }
            else
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), component.WorldPickupItem);
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
            if (TechGroup.Lockers.Contains(ev.TechType))
            {
                ev.IsAllowed = false;

                StorageProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
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
            if (TechGroup.Lockers.Contains(ev.TechType))
            {
                ev.IsAllowed = false;

                StorageProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem = null, bool isAdded = false)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.StorageLocker()
                {
                    IsAdded         = isAdded,
                    WorldPickupItem = pickupItem,
                }
            };

            NetworkClient.SendPacket(result);
        }

        /**
         *
         * Başlangıç depo eşyalarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool InitializeStorage(TechType techType, string uniqueId, MetadataComponent packet, global::ItemsContainer itemsContainer)
        {
            var component = packet.GetComponent<Metadata.StorageContainer>();
            if (component == null)
            {
                return false;
            }

            if (component.Sign != null)
            {
                MetadataProcessor.ExecuteProcessor(TechType.Sign, uniqueId, component.Sign, true);
            }

            foreach(var item in component.Items)
            {
                Entity.SpawnToQueue(item.Item, item.ItemId, itemsContainer);
            }
            
            return true;
        }
    }
}
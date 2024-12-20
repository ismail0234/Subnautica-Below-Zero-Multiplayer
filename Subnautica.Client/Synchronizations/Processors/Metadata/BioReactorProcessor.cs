namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BioReactorProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BioReactor>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                Network.Storage.InitializeStorage(uniqueId, component.StorageContainer);
                return true;
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
            if (ev.TechType == TechType.BaseBioReactor)
            {
                ev.IsAllowed = false;

                BioReactorProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
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
                Component = new Metadata.BioReactor()
                {
                    IsAdded         = isAdded,
                    WorldPickupItem = pickupItem,
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class CoffeeVendingMachineProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.CoffeeVendingMachine>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::CoffeeVendingMachine>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            if (isSilence)
            {
                foreach(var thermos in component.Thermoses.Where(q => q.IsActive))
                {
                    this.ProcessThermos(thermos.ItemId, gameObject.storageContainer.container, thermos.IsFull, true);
                }

                return true;
            }

            if (component.IsAdding && ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
            {
                World.DestroyItemFromPlayer(component.ItemId);
            }

            this.ProcessThermos(component.ItemId, gameObject.storageContainer.container, component.IsFull, component.IsAdding, ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));
            return true;
        }

        /**
         *
         * Termosları işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ProcessThermos(string itemId, ItemsContainer container, bool isFull, bool isAdding, bool isMine = false)
        {
            if (isAdding)
            {
                if (isFull)
                {
                    Entity.SpawnToQueue(TechType.Coffee, itemId, container, new ItemQueueAction(this.OnFullEntitySpawning, this.OnFullEntitySpawned));
                }
                else
                {
                    Entity.SpawnToQueue(TechType.Coffee, itemId, container);
                }
            }
            else
            {
                Entity.RemoveToQueue(itemId);

                if (isMine)
                {
                    if (isFull)
                    {
                        Entity.SpawnToQueue(TechType.Coffee, itemId, global::Inventory.Get().container, new ItemQueueAction(null, this.OnFullEntitySpawned));
                    }
                    else
                    {
                        Entity.SpawnToQueue(TechType.Coffee, itemId, global::Inventory.Get().container, new ItemQueueAction());
                    }
                }
            }
        }

        /**
         *
         * Nesne spawnlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnFullEntitySpawning(ItemQueueProcess item)
        {
            var thermos = Network.Identifier.GetComponentByGameObject<global::Thermos>(item.ItemId, true);
            if (thermos == null)
            {
                return true;
            }

            thermos.eatable.Refill();
            return false;
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFullEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            pickupable.GetComponent<global::Eatable>().Refill();
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
            if (ev.TechType == TechType.CoffeeVendingMachine)
            {
                ev.IsAllowed = false;

                CoffeeVendingMachineProcessor.SendPacketToServer(ev.UniqueId, ev.ItemId, isAdding: true);
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
            if (ev.TechType == TechType.CoffeeVendingMachine)
            {
                ev.IsAllowed = false;

                CoffeeVendingMachineProcessor.SendPacketToServer(ev.UniqueId, ev.ItemId, WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer),  false);
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string itemId, WorldPickupItem pickupItem = null, bool isAdding = false)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.CoffeeVendingMachine()
                {
                    IsAdding   = isAdding,
                    ItemId     = itemId,
                    PickupItem = pickupItem,
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}
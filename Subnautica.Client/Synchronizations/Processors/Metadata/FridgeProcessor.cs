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

    public class FridgeProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Fridge>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                if (component.StorageContainer != null)
                {
                    var gameObject = Network.Identifier.GetComponentByGameObject<global::Fridge>(uniqueId);
                    if (gameObject)
                    {
                        foreach (var item in component.StorageContainer.Items)
                        {
                            component.ItemComponent = component.Components.FirstOrDefault(q => q.ItemId == item.ItemId);

                            var action = new ItemQueueAction();
                            action.OnEntitySpawned = this.OnEntitySpawned;
                            action.RegisterProperty("CustomProperty", component);

                            Entity.SpawnToQueue(item.Item, item.ItemId, gameObject.storageContainer.container, action);
                        }
                    }
                }

                return true;
            }

            if (component.IsPowerStateChanged)
            {
                var action = new ItemQueueAction();
                action.OnProcessCompleted = this.OnProcessCompleted;
                action.RegisterProperty("UniqueId", uniqueId);
                action.RegisterProperty("Fridge"  , component);

                Entity.ProcessToQueue(action);
            }
            else
            {
                if (component.IsAdded)
                {
                    Network.Storage.AddItemToStorage(uniqueId, packet.GetPacketOwnerId(), component.WorldPickupItem, this.OnEntitySpawned, component);
                }
                else
                {
                    Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), component.WorldPickupItem, this.OnEntitySpawned, component);
                }
            }

            return true;
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnProcessCompleted(ItemQueueProcess item)
        {
            var gameObject = Network.Identifier.GetComponentByGameObject<global::Fridge>(item.Action.GetProperty<string>("UniqueId"));
            if (gameObject == null)
            {
                var fridge = item.Action.GetProperty<Metadata.Fridge>("Fridge");

                foreach (var inventoryItem in gameObject.storageContainer.container)
                {
                    if (inventoryItem.item.TryGetComponent<Eatable>(out var eatable) && eatable.decomposes)
                    {
                        if (fridge.WasPowered)
                        {
                            eatable.decayPaused    = true;
                            eatable.timeDecayPause = fridge.CurrentTime;
                        }
                        else
                        {
                            eatable.decayPaused = false;
                            eatable.timeDecayStart += (fridge.CurrentTime - eatable.timeDecayPause);
                        }
                    }
                }
            }
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            var fridge = item.Action.GetProperty<Metadata.Fridge>("CustomProperty");
            if (fridge.ItemComponent != null && fridge.ItemComponent.IsDecomposes)
            {
              if (pickupable.TryGetComponent<global::Eatable>(out var eatable))
                {
                    if (fridge.ItemComponent.IsPaused)
                    {
                        eatable.decayPaused = true;
                        eatable.timeDecayPause = fridge.ItemComponent.TimeDecayPause;
                        eatable.timeDecayStart = fridge.ItemComponent.TimeDecayStart;
                    }
                    else
                    {
                        eatable.decayPaused    = false;
                        eatable.timeDecayStart = fridge.ItemComponent.TimeDecayStart + (fridge.CurrentTime - fridge.ItemComponent.TimeDecayPause);
                    }
                }
            }
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
            if (ev.TechType == TechType.Fridge)
            {
                ev.IsAllowed = false;

                if (ev.Item.TryGetComponent<global::Eatable>(out var eatable))
                {
                    FridgeProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), eatable.decomposes, eatable.timeDecayStart, true);
                }
                else
                {
                    FridgeProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
                }
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
            if (ev.TechType == TechType.Fridge)
            {
                ev.IsAllowed = false;

                FridgeProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer), isAdded: false);
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem = null, bool isDecomposes = false, float timeDecayStart = 0f, bool isAdded = false)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Fridge()
                {
                    IsAdded         = isAdded,
                    WorldPickupItem = pickupItem,
                    IsDecomposes    = isDecomposes,
                    TimeDecayStart  = timeDecayStart,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
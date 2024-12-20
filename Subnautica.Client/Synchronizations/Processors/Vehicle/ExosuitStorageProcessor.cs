namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ExosuitStorageProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ExosuitStorageArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (packet.IsPickup)
            {
                World.DestroyPickupItem(packet.WorldPickupItem);

                var action = new ItemQueueAction();
                action.RegisterProperty("Component", packet);
                action.RegisterProperty("IsMine"   , ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));
                action.OnProcessCompleted = this.OnProcessCompleted;
                action.OnEntitySpawned    = this.OnEntityPickupSpawned;

                Entity.ProcessToQueue(action);
            }
            else
            {
                if (packet.IsAdded)
                {
                    Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), packet.WorldPickupItem);
                }
                else
                {
                    Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), packet.WorldPickupItem);
                }
            }

            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnProcessCompleted(ItemQueueProcess item)
        {
            var component = item.Action.GetProperty<ServerModel.ExosuitStorageArgs>("Component");
            if (component != null)
            {
                var exosuit = Network.Identifier.GetComponentByGameObject<global::Exosuit>(component.UniqueId);
                if (exosuit)
                {
                    World.SpawnPickupItem(component.WorldPickupItem, exosuit.storageContainer.container, item.Action);
                }
            }
        }

        /**
         *
         * Bir nesne alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityPickupSpawned(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            if (item.Action.GetProperty<bool>("IsMine"))
            {
                pickupable.MultiplayerPlayPickupSound();
            }
        }

        /**
         *
         * Exosuit ile yerden nesne alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnExosuitItemPickedUp(ExosuitItemPickedUpEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.ItemId))
            {
                ExosuitStorageProcessor.SendPacketToServer(ev.UniqueId, isPickup: true, pickupItem: WorldPickupItem.Create(ev.Item));
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
            if (ev.TechType == TechType.Exosuit)
            {
                ev.IsAllowed = false;

                ExosuitStorageProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
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
            if (ev.TechType == TechType.Exosuit)
            {
                ev.IsAllowed = false;

                ExosuitStorageProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Spy Penguin bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem = null, bool isPickup = false, bool isAdded = false)
        {
            ServerModel.ExosuitStorageArgs request = new ServerModel.ExosuitStorageArgs()
            {
                UniqueId        = uniqueId,
                IsPickup        = isPickup,
                IsAdded         = isAdded,
                WorldPickupItem = pickupItem,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class SpyPenguinProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPlayerItemComponent packet, byte playerId)
        {
            var component = packet.GetComponent<ItemModel.SpyPenguin>();
            if (component == null)
            {
                return false;
            }

            if (component.IsDeploy)
            {
                if (ZeroPlayer.IsPlayerMine(playerId))
                {
                    World.DestroyItemFromPlayer(component.Entity.UniqueId);
                }

                Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned);
            }
            else if (component.IsStalkerFur || component.IsPickup)
            {
                World.DestroyPickupItem(component.WorldPickupItem);

                var action = new ItemQueueAction();
                action.RegisterProperty("Component", component);
                action.RegisterProperty("IsMine"   , ZeroPlayer.IsPlayerMine(playerId));
                action.OnProcessCompleted = this.OnProcessCompleted;
                action.OnEntitySpawned    = this.OnEntityPickupSpawned;

                Entity.ProcessToQueue(action);
            }
            else
            {
                if (component.IsAdded)
                {
                    Network.Storage.AddItemToStorage(packet.UniqueId, playerId, component.WorldPickupItem);
                }
                else
                {
                    Network.Storage.AddItemToInventory(playerId, component.WorldPickupItem);
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
            var component = item.Action.GetProperty<ItemModel.SpyPenguin>("Component");
            if (component != null)
            {
                var penguin = Network.Identifier.GetComponentByGameObject<global::SpyPenguin>(component.UniqueId);
                if (penguin)
                {
                    World.SpawnPickupItem(component.WorldPickupItem, penguin.inventory.container, item.Action);
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
                pickupable.ShowPickupNotify();

                var component = item.Action.GetProperty<ItemModel.SpyPenguin>("Component");
                var penguin   = Network.Identifier.GetComponentByGameObject<global::SpyPenguin>(component.UniqueId);
                if (penguin)
                {
                    penguin.PlayVO(penguin.sfx_pickupSuccess);
                }

                if (uGUI_SpyPenguin.main.GetPenguin() == penguin)
                {
                    uGUI_SpyPenguin.main.SetInventory(penguin.inventory.container.count);
                }
            }
        }

        /**
         *
         * Nesne doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);
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
            if (ev.TechType == TechType.SpyPenguin)
            {
                ev.IsAllowed = false;

                SpyPenguinProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
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
            if (ev.TechType == TechType.SpyPenguin)
            {
                ev.IsAllowed = false;

                SpyPenguinProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }
        
        /**
         *
         * Spy Penguin kar avcısından kar kürkü alırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinSnowStalkerInteracting(SpyPenguinSnowStalkerInteractingEventArgs ev)
        {
            ev.IsAllowed = false;

            SpyPenguinProcessor.SendPacketToServer(ev.UniqueId, isStalkerFur: true, spawnChance: ev.SpawnChance);
        }

        /**
         *
         * Spy Penguin bir nesne aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinItemPickedUp(SpyPenguinItemPickedUpEventArgs ev)
        {
            ev.IsAllowed = false;

            SpyPenguinProcessor.SendPacketToServer(ev.UniqueId, isPickup: true, pickupItem: WorldPickupItem.Create(ev.Item));
        }

        /**
         *
         * Spy Penguin bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinDeploying(SpyPenguinDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            SpyPenguinProcessor.SendPacketToServer(ev.UniqueId, isDeploy: true, position: ev.Position.ToZeroVector3(), rotation: ev.Rotation.ToZeroQuaternion(), name: ev.Name, health: ev.Health);
        }

        /**
         *
         * Spy Penguin bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, ZeroVector3 position = null, ZeroQuaternion rotation = null, string name = null, float health = 0f, WorldPickupItem pickupItem = null, float spawnChance = -1, bool isPickup = false, bool isStalkerFur = false, bool isDeploy = false, bool isAdded = false)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.SpyPenguin()
                {
                    UniqueId        = uniqueId,
                    Position        = position,
                    Rotation        = rotation,
                    Name            = name,
                    Health          = health,
                    WorldPickupItem = pickupItem,
                    SpawnChance     = spawnChance,
                    IsPickup        = isPickup,
                    IsStalkerFur    = isStalkerFur,
                    IsDeploy        = isDeploy,
                    IsAdded         = isAdded,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
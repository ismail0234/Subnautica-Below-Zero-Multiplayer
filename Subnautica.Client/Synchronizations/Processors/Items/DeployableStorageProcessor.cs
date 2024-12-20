namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Synchronizations.Processors.Player;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class DeployableStorageProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.DeployableStorage>();
            if (component == null)
            {
                return false;
            }

            if (component.IsSignProcess)
            {
                if (!component.IsSignSelect)
                {
                    using (EventBlocker.Create(TechType.Sign))
                    {
                        var coloredLabel = Network.Identifier.GetComponentByGameObject<global::ColoredLabel>(packet.UniqueId);
                        if (coloredLabel)
                        {
                            coloredLabel.signInput.text       = component.SignText;
                            coloredLabel.signInput.colorIndex = component.SignColorIndex;
                        }
                    }
                }
            }
            else
            {
                if (component.IsAdded)
                {
                    if (component.TechType == TechType.QuantumLocker)
                    {
                        Network.Storage.AddItemToStorage(QuantumLockerStorage.GetStorageContainer(true).gameObject.GetIdentityId(), playerId, component.WorldPickupItem);
                    }
                    else
                    {
                        Network.Storage.AddItemToStorage(packet.UniqueId, playerId, component.WorldPickupItem);
                    }
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
         * Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemAdding(StorageItemAddingEventArgs ev)
        {
            if (ev.TechType == TechType.SmallStorage || ev.TechType == TechType.QuantumLocker)
            {
                ev.IsAllowed = false;

                DeployableStorageProcessor.SendPacketToServer(ev.UniqueId, techType: ev.TechType, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
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
            if (ev.TechType == TechType.SmallStorage || ev.TechType == TechType.QuantumLocker)
            {
                ev.IsAllowed = false;

                DeployableStorageProcessor.SendPacketToServer(ev.UniqueId, techType: ev.TechType, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Su geçirmez depoyu bıraktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDeployableStorageDeploying(DeployableStorageDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            ItemDropProcessor.SendPacketToServer(ev.UniqueId, WorldPickupItem.Create(ev.Pickupable, PickupSourceType.PlayerInventoryDrop, true), ev.DeployPosition.ToZeroVector3(), Quaternion.identity.ToZeroQuaternion(), ev.Forward.ToZeroVector3());
        }

        /**
         *
         * Tabela seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignSelect(SignSelectEventArgs ev)
        {
            if (ev.TechType == TechType.SmallStorage)
            {
                if (Interact.IsBlocked(ev.UniqueId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    DeployableStorageProcessor.SendPacketToServer(ev.UniqueId, techType: ev.TechType, isSignProcess: true, isSignSelect: true);
                }
            }
        }

        /**
         *
         * Tabela da veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignDataChanged(SignDataChangedEventArgs ev)
        {
            if (ev.TechType == TechType.SmallStorage)
            {
                DeployableStorageProcessor.SendPacketToServer(ev.UniqueId, techType: ev.TechType, isSignProcess: true, signText: ev.Text, signColorIndex: ev.ColorIndex);
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, TechType techType = TechType.None, bool isSignProcess = false, bool isSignSelect = false, string signText = null, int signColorIndex = -1, WorldPickupItem pickupItem = null, bool isAdded = false)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.DeployableStorage()
                {
                    UniqueId        = uniqueId,
                    TechType        = techType,
                    IsSignProcess   = isSignProcess,
                    IsSignSelect    = isSignSelect,
                    IsAdded         = isAdded,
                    SignText        = signText,
                    SignColorIndex  = signColorIndex,
                    WorldPickupItem = pickupItem,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
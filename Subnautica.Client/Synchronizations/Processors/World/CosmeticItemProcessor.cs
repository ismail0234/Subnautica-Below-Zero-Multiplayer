namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CosmeticItemProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CosmeticItemArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            if (packet.PickupItem != null)
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), packet.PickupItem);
            }
            else
            {
                if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                {
                    World.DestroyItemFromPlayer(packet.UniqueId);
                }

                var action = new ItemQueueAction(null, this.OnEntitySpawned);
                action.RegisterProperty("BaseId", packet.BaseId);
                action.RegisterProperty("Item"  , packet.CosmeticItem);

                Entity.SpawnToQueue(packet.TechType, packet.UniqueId, new ZeroTransform(packet.Position, packet.Rotation), action);
            }

            return true;
        }

        /**
         *
         * Nesne dünyaya yerleştirildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerPlace(item.Action.GetProperty<string>("BaseId"));

            Network.Session.SetCosmeticItem(item.Action.GetProperty<CosmeticItem>("Item"));
        }

        /**
         *
         * Oyuncu yerden eşya aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerItemPickedUp(PlayerItemPickedUpEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity && Network.Session.IsCosmeticItemExists(ev.UniqueId))
            {
                ev.IsAllowed = false;

                CosmeticItemProcessor.SendPacketToServer(ev.UniqueId, techType: ev.TechType, pickupItem: WorldPickupItem.Create(ev.Pickupable, PickupSourceType.CosmeticItem));
            }
        }

        /**
         *
         * Kozmetik dünyaya yerleştirilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCosmeticItemPlacing(CosmeticItemPlacingEventArgs ev)
        {
            ev.IsAllowed = false;

            CosmeticItemProcessor.SendPacketToServer(ev.UniqueId, ev.BaseId, ev.TechType, ev.Position.ToZeroVector3(), ev.Rotation.ToZeroQuaternion());
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string baseId = null, TechType techType = TechType.None, ZeroVector3 position = null, ZeroQuaternion rotation = null, WorldPickupItem pickupItem = null)
        {
            ServerModel.CosmeticItemArgs request = new ServerModel.CosmeticItemArgs()
            {
                UniqueId = uniqueId,
                BaseId   = baseId,
                TechType = techType,
                Position = position,
                Rotation = rotation,
                PickupItem = pickupItem,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
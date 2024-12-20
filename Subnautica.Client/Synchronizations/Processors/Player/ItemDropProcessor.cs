namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.Synchronizations.Processors.Items;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ItemDropProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ItemDropArgs>();
            if (packet.Entity == null)
            {
                return false;
            }

            if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
            {
                World.DestroyItemFromPlayer(packet.Entity.UniqueId, true);
            }

            Network.DynamicEntity.Spawn(packet.Entity, this.OnEntitySpawned, packet.Forward);
            return true;
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
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                pickupable.MultiplayerDrop(true);

                var forward = item.Action.GetProperty<ZeroVector3>("CustomProperty");
                if (forward != null && pickupable.TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
                {
                    rb.AddForce(forward.ToVector3(), ForceMode.VelocityChange);
                }
            }
        }

        /**
         *
         * Oyuncu bir nesneyi bırakırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerItemDroping(PlayerItemDropingEventArgs ev)
        {
            if (ev.Item.GetTechType() != TechType.SnowBall && !ev.Item.GetTechType().IsCreature())
            {
                if (ev.Item.GetTechType().IsCreatureEgg() && ZeroPlayer.CurrentPlayer.GetCurrentWaterParkUniqueId().IsNotNull())
                {
                    return;
                }

                ev.IsAllowed = false;

                if (IsSpecialItemDrop(ev, ev.Item.GetTechType()) == false)
                {
                    ItemDropProcessor.SendPacketToServer(ev.UniqueId, WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventoryDrop), ev.Position.ToZeroVector3(), ev.Rotation.ToZeroQuaternion());
                }
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem, ZeroVector3 position, ZeroQuaternion rotation, ZeroVector3 forward = null)
        {
            ServerModel.ItemDropArgs request = new ServerModel.ItemDropArgs()
            {
                WorldPickupItem = pickupItem,
                Forward         = forward,
                Entity          = new WorldDynamicEntity()
                {
                    UniqueId = uniqueId,
                    Position = position,
                    Rotation = rotation,
                }
            };

            NetworkClient.SendPacket(request);
        }

        /**
         *
         * Yere bırakma pasif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsSpecialItemDrop(PlayerItemDropingEventArgs ev, TechType techType)
        {
            if (techType == TechType.Beacon)
            {
                if (ev.Item.TryGetComponent<global::Beacon>(out var beacon))
                {
                    BeaconProcessor.SendPacketToServer(ev.UniqueId, ev.Position.ToZeroVector3(), ev.Rotation.ToZeroQuaternion(), !global::Player.main.IsUnderwater(), beacon.beaconLabel.labelName);
                }

                return true;
            }

            switch (techType)
            {

                case TechType.SpyPenguin:
                case TechType.Flare:
                case TechType.Hoverbike:
                case TechType.MapRoomCamera:
                case TechType.PipeSurfaceFloater:
                    return true;
            }

            return false;
        }
    }
}

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

    using ItemModel        = Subnautica.Network.Models.Items;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class MapRoomCameraProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.DroneCamera>();
            if (component == null)
            {
                return false;
            }

            if (ZeroPlayer.IsPlayerMine(playerId))
            {
                World.DestroyItemFromPlayer(component.Entity.UniqueId, true);
            }

            Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned, component.Forward);
            return true;
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
            var forward = item.Action.GetProperty<ZeroVector3>("CustomProperty");
            var entity  = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (forward != null && pickupable.TryGetComponent<Rigidbody>(out var component) && !component.isKinematic)
            {
                component.AddForce(forward.ToVector3(), ForceMode.VelocityChange);
            }

            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);
            }
        }

        /**
         *
         * Drone camera denize bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDroneCameraDeploying(DroneCameraDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            MapRoomCameraProcessor.SendPacketToServer(ev.UniqueId, WorldPickupItem.Create(ev.Pickupable, PickupSourceType.PlayerInventoryDrop), ev.DeployPosition.ToZeroVector3(), ev.Forward.ToZeroVector3(), Quaternion.identity.ToZeroQuaternion(), ev.Pickupable.ToMapRoomCameraComponent());
        }

        /**
         *
         * Sunucuya paket gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, WorldPickupItem pickupItem, ZeroVector3 position, ZeroVector3 forward, ZeroQuaternion rotation, WorldEntityModel.MapRoomCamera component)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.DroneCamera()
                {
                    UniqueId   = uniqueId,
                    PickupItem = pickupItem,
                    Position   = position,
                    Forward    = forward,
                    Rotation   = rotation,
                    Component  = component,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
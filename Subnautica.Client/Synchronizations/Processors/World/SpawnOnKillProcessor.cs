namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SpawnOnKillProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SpawnOnKillArgs>();
            if (packet == null)
            {
                return false;
            }
            
            World.DestroyPickupItem(packet.WorldPickupItem);
            
            Network.DynamicEntity.Spawn(packet.Entity, this.OnEntitySpawned);
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
            pickupable.MultiplayerDrop();
        }

        /**
         *
         * Bir nesne yok edildiğinde içinden başka nesne çıkarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpawnOnKilling(SpawnOnKillingEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.SpawnOnKillArgs request = new ServerModel.SpawnOnKillArgs()
            {
                WorldPickupItem = WorldPickupItem.Create(StorageItem.Create(ev.UniqueId, ev.TechType)),
                Entity          = new WorldDynamicEntity()
                {
                    Position = ev.Position.ToZeroVector3(),
                    Rotation = ev.Rotation.ToZeroQuaternion(),
                }
            };

            NetworkClient.SendPacket(request);
        }
    }
}
namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class ThumperProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.Thumper>();
            if (component == null)
            {
                return false;
            }

            if (ZeroPlayer.IsPlayerMine(playerId))
            {
                World.DestroyItemFromPlayer(component.Entity.UniqueId);
            }

            Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned);
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
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);
            }
        }

        /**
         *
         * Thumper yere konulurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnThumperDeploying(ThumperDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            ThumperProcessor.SendPacketToServer(ev.UniqueId, ev.DeployPosition.ToZeroVector3(), Quaternion.identity.ToZeroQuaternion(), ev.Charge);
        }

        /**
         *
         * Thumper yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, ZeroVector3 position, ZeroQuaternion rotation, float charge)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Thumper()
                {
                    UniqueId = uniqueId,
                    Position = position,
                    Rotation = rotation,
                    Charge   = charge,
                }
            };

            NetworkClient.SendPacket(result);   
        }
    }
}
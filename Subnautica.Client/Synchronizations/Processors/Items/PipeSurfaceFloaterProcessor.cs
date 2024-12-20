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
    using EntityModel = Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities;

    public class PipeSurfaceFloaterProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.PipeSurfaceFloater>();
            if (component == null)
            {
                return false;
            }

            if (component.IsSurfaceFloaterDeploy())
            {
                if (ZeroPlayer.IsPlayerMine(playerId))
                {
                    World.DestroyItemFromPlayer(component.Entity.UniqueId, true);
                }

                Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned);
            }
            else if (component.IsOxygenPipePlace())
            {
                if (ZeroPlayer.IsPlayerMine(playerId))
                {
                    World.DestroyItemFromPlayer(component.PipeId, true);
                }

                EntityModel.PipeSurfaceFloaterProcessor.SpawnOxygenPipe(component.ParentId, component.PipeId, component.Position);
            }
            else if (component.IsOxygenPipePickup())
            {
                Network.Storage.AddItemToInventory(playerId, component.PickupItem);
            }

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
         * Boru yüzey yüzdürücü bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPipeSurfaceFloaterDeploying(PipeSurfaceFloaterDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            PipeSurfaceFloaterProcessor.SendPacketToServer(ev.UniqueId, null, null, ev.DeployPosition.ToZeroVector3(), ev.DeployRotation.ToZeroQuaternion(), processType: 1);
        }

        /**
         *
         * Boru bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOxygenPipePlacing(OxygenPipePlacingEventArgs ev)
        {
            ev.IsAllowed = false;

            PipeSurfaceFloaterProcessor.SendPacketToServer(ev.UniqueId, ev.ParentId, ev.PipeId, ev.DeployPosition.ToZeroVector3(), ev.DeployRotation.ToZeroQuaternion(), processType: 3);
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
            if (!ev.IsStaticWorldEntity && ev.TechType == TechType.Pipe && !Network.DynamicEntity.HasEntity(ev.UniqueId))
            {
                if (ev.Pickupable.TryGetComponent<OxygenPipe>(out var oxygenPipe) && oxygenPipe.GetRoot() != null)
                {
                    ev.IsAllowed = false;

                    PipeSurfaceFloaterProcessor.SendPacketToServer(oxygenPipe.GetRoot().GetGameObject().GetIdentityId(), pipeId: ev.UniqueId, processType: 4);
                }
            }
        }

        /**
         *
         * Sunucuya paket gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, string parentId = null, string pipeId = null, ZeroVector3 position = null, ZeroQuaternion rotation = null, byte processType = 0)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.PipeSurfaceFloater()
                {
                    UniqueId    = uniqueId,
                    ParentId    = parentId,
                    PipeId      = pipeId,
                    Position    = position,
                    Rotation    = rotation,
                    ProcessType = processType
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
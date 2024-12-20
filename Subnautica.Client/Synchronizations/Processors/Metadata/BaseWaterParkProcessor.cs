namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class BaseWaterParkProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BaseWaterPark>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                return true;
            }

            if (component.ProcessType == Metadata.BaseWaterParkProcessType.ItemDrop)
            {
                if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                {
                    World.DestroyItemFromPlayer(component.Entity.UniqueId, true);
                }

                Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned, uniqueId);
            }

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
            var uniqueId = item.Action.GetProperty<string>("CustomProperty");
            var entity   = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                var component = entity.Component.GetComponent<WorldEntityModel.WaterParkCreature>();
                if (component != null)
                {
                    pickupable.MultiplayerDrop(true, waterParkId: uniqueId, waterParkAddTime: component.AddedTime);
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
            if (ev.Item.GetTechType().IsCreatureEgg())
            {
                var waterParkId = ZeroPlayer.CurrentPlayer.GetCurrentWaterParkUniqueId();
                if (waterParkId.IsNotNull())
                {
                    ev.IsAllowed = false;

                    SendPacketToServer(waterParkId, ev.UniqueId, ev.Item.GetTechType(), ev.Position.ToZeroVector3(), ev.Rotation.ToZeroQuaternion(), Metadata.BaseWaterParkProcessType.ItemDrop);
                }
            }
        }

        /**
         *
         * Depolama'dan eşya kaldırıldırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string itemId, TechType techType, ZeroVector3 position, ZeroQuaternion rotation, Metadata.BaseWaterParkProcessType processType)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.BaseWaterPark()
                {
                    ProcessType = processType,
                    Entity      = new WorldDynamicEntity()
                    {
                        UniqueId = itemId,
                        TechType = techType,
                        Position = position,
                        Rotation = rotation,
                    }
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
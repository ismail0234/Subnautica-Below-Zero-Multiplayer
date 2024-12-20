namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BeaconProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.Beacon>();
            if (component == null)
            {
                return false;
            }

            if (component.IsTextChanged)
            {
                var action = new ItemQueueAction();
                action.OnProcessCompleted = OnEntityProcessCompleted;
                action.RegisterProperty("UniqueId", component.UniqueId);
                action.RegisterProperty("Text"    , component.Text);

                Entity.ProcessToQueue(action);
            }
            else
            {
                if (ZeroPlayer.IsPlayerMine(playerId))
                {
                    World.DestroyItemFromPlayer(component.Entity.UniqueId, true);
                }

                Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned);
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
         * Nesne işlemi tamamlanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityProcessCompleted(ItemQueueProcess item)
        {
            var uniqueId = item.Action.GetProperty<string>("UniqueId");
            var text     = item.Action.GetProperty<string>("Text");

            var gameObject = Network.Identifier.GetComponentByGameObject<global::BeaconLabel>(uniqueId);
            if (gameObject)
            {
                using (EventBlocker.Create(TechType.Beacon))
                {
                    gameObject.SetLabel(text);
                }
            }
        }

        /**
         *
         * Beacon adı değişince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBeaconLabelChanged(BeaconLabelChangedEventArgs ev)
        {
            BeaconProcessor.SendPacketToServer(ev.UniqueId, text: ev.Text, isTextChanged: true);
        }

        /**
         *
         * Beacon yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBeaconDeploying(BeaconDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            BeaconProcessor.SendPacketToServer(ev.UniqueId, ev.DeployPosition.ToZeroVector3(), ev.DeployRotation.ToZeroQuaternion(), ev.IsDeployedOnLand, ev.Text);
        }

        /**
         *
         * Sunucuya paket gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, ZeroVector3 position = null, ZeroQuaternion rotation = null, bool isDeployedOnLand = false, string text = null, bool isTextChanged = false)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Beacon()
                {
                    UniqueId         = uniqueId,
                    Position         = position,
                    Rotation         = rotation,
                    Text             = text,
                    IsDeployedOnLand = isDeployedOnLand,
                    IsTextChanged    = isTextChanged,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
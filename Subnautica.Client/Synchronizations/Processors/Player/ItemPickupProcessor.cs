namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ItemPickupProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ItemPickupArgs>();
            if (packet.WorldPickupItem.Item.ItemId.IsNull())
            {
                return false;
            }
            
            if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
            {
                var action = new ItemQueueAction();
                action.OnProcessCompleted = this.OnProcessCompleted;
                action.RegisterProperty("ItemId", packet.WorldPickupItem.Item.ItemId);
                
                Entity.ProcessToQueue(action);
            }
            else
            {
                Network.DynamicEntity.Remove(packet.WorldPickupItem.Item.ItemId);
            }

            return true;
        }

        /**
         *
         * Nesne işlemi tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnProcessCompleted(ItemQueueProcess item)
        {
            var itemId = item.Action.GetProperty<string>("ItemId");
            if (itemId.IsNotNull())
            {
                Network.DynamicEntity.RemoveEntity(itemId);

                var pickupable = Network.Identifier.GetComponentByGameObject<Pickupable>(itemId);
                if (pickupable)
                {
                    pickupable.LocalPickup();
                }
            }
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
            if (!ev.IsStaticWorldEntity && Network.DynamicEntity.HasEntity(ev.UniqueId))
            {
                ev.IsAllowed = false;

                ServerModel.ItemPickupArgs request = new ServerModel.ItemPickupArgs()
                {
                    WorldPickupItem = WorldPickupItem.Create(ev.Pickupable)
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}

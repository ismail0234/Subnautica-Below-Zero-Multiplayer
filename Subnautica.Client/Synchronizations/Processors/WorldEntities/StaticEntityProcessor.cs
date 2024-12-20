namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class StaticEntityProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StaticEntityPickedUpArgs>();
            if (packet == null)
            {
                return false;
            }

            if (packet.Entity != null)
            {
                Network.StaticEntity.AddStaticEntity(packet.Entity);
            }

            if (packet.UniqueId.IsNull())
            {
                Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), packet.WorldPickupItem, showNotify: true);
            }
            else
            {
                if (packet.WorldPickupItem != null && packet.WorldPickupItem.Item.TechType == TechType.Snowman)
                {
                    var gameObject = Network.Identifier.GetComponentByGameObject<global::Snowman>(packet.UniqueId);
                    if (gameObject)
                    {
                        using (EventBlocker.Create(TechType.Snowman))
                        {
                            gameObject.OnHandClick(null);
                        }
                    }
                }
                else
                {
                    if (!ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                    {
                        Entity.RemoveToQueue(packet.Entity.UniqueId);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Alterra pda nesnesi aldıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAlterraPdaPickedUp(AlterraPdaPickedUpEventArgs ev)
        {
            StaticEntityProcessor.SendPacketToServer(ev.UniqueId);
        }

        /**
         *
         * Müzik nesnesi alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxDiskPickedUp(JukeboxDiskPickedUpEventArgs ev)
        {
            StaticEntityProcessor.SendPacketToServer(ev.UniqueId);
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
            if (ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;

                StaticEntityProcessor.SendPacketToServer(worldPickupItem: WorldPickupItem.Create(ev.Pickupable));
            }
        }

        /**
         *
         * Kardan adam yok edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSnowmanDestroying(SnowmanDestroyingEventArgs ev)
        {
            if (ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;

                StaticEntityProcessor.SendPacketToServer(ev.UniqueId, worldPickupItem: WorldPickupItem.Create(StorageItem.Create(ev.UniqueId, TechType.Snowman), PickupSourceType.Static));
            }
        }

        /**
         *
         * Müzik nesnesi alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId = null, WorldPickupItem worldPickupItem = null)
        {
            ServerModel.StaticEntityPickedUpArgs request = new ServerModel.StaticEntityPickedUpArgs()
            {
                UniqueId        = uniqueId,
                WorldPickupItem = worldPickupItem
            };

            NetworkClient.SendPacket(request);
        }
    }
}
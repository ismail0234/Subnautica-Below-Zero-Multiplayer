namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using EntityModel = Subnautica.Network.Models.WorldEntity;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BulkheadDoorProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkWorldEntityComponent packet, byte requesterId, bool isSpawning)
        {
            var entity = packet.GetComponent<EntityModel.BulkheadDoor>();
            if (string.IsNullOrEmpty(entity.UniqueId))
            {
                return false;
            }

            Network.StaticEntity.AddStaticEntity(entity);

            if (isSpawning)
            {
                var gameObject = Network.Identifier.GetComponentByGameObject<global::BulkheadDoor>(entity.UniqueId);
                if (gameObject && gameObject.opened != entity.IsOpened)
                {
                    gameObject.SetInitialyOpen(entity.IsOpened);
                }

                return true;
            }

            var player = ZeroPlayer.GetPlayerById(requesterId);
            if (player == null)
            {
                return false;
            }

            if (player.IsMine)
            {
                player.OnHandClickBulkhead(packet.UniqueId, entity.IsOpened, entity.Side);
            }
            else
            {
                if (entity.IsOpened)
                {
                    player.OpenStartCinematicBulkhead(packet.UniqueId, entity.Side);
                }
                else
                {
                    player.CloseStartCinematicBulkhead(packet.UniqueId, entity.Side);
                }
            }

            return true;
        }

        /**
         *
         * Bölme kapısı açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBulkheadOpening(BulkheadOpeningEventArgs ev)
        {
            if (ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;

                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    BulkheadDoorProcessor.SendPacketToServer(ev.UniqueId, ev.Side, true, ev.StoryCinematicType);
                }
            }
        }

        /**
         *
         * Bölme kapısı kapanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBulkheadClosing(BulkheadClosingEventArgs ev)
        {
            if (ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;
                
                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    BulkheadDoorProcessor.SendPacketToServer(ev.UniqueId, ev.Side, false);
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
        public static void SendPacketToServer(string uniqueId, bool side, bool isOpened, StoryCinematicType storyCinematicType = StoryCinematicType.None)
        {
            ServerModel.WorldEntityActionArgs request = new ServerModel.WorldEntityActionArgs()
            {
                Entity = new EntityModel.BulkheadDoor()
                {
                    UniqueId           = uniqueId,
                    Side               = side,
                    IsOpened           = isOpened,
                    StoryCinematicType = storyCinematicType,
                },
            };

            NetworkClient.SendPacket(request);
        }
    }
}
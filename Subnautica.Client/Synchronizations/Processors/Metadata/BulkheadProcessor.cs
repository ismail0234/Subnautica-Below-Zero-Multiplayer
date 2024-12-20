namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class BulkheadProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BulkheadDoor>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                var gameObject = Network.Identifier.GetComponentByGameObject<global::BulkheadDoor>(uniqueId);
                if (gameObject && gameObject.opened != component.IsOpened)
                {
                    gameObject.SetState(component.IsOpened);
                }

                return true;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player == null)
            {
                return false;
            }

            if (player.IsMine)
            {
                player.OnHandClickBulkhead(packet.UniqueId, component.IsOpened, component.Side);
            }
            else
            {
                if (component.IsOpened)
                {
                    player.OpenStartCinematicBulkhead(packet.UniqueId, component.Side);
                }
                else
                {
                    player.CloseStartCinematicBulkhead(packet.UniqueId, component.Side);
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
            if (!ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;
                
                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    BulkheadProcessor.SendPacketToServer(ev.UniqueId, ev.Side, true);
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
            if (!ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;
                
                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    BulkheadProcessor.SendPacketToServer(ev.UniqueId, ev.Side, false);
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
        public static void SendPacketToServer(string uniqueId, bool side, bool isOpened)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.BulkheadDoor(isOpened, side)
            };

            NetworkClient.SendPacket(result);
        }
    }
}
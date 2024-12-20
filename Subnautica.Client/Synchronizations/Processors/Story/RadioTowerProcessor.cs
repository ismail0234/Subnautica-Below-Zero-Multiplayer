namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class RadioTowerProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryRadioTowerArgs>();
            if (packet.IsTOMUsing)
            {
                var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
                if (packet != null && player.IsMine)
                {
                    player.OnHandClickRadioTowerInsertedItem(packet.UniqueId);
                }
                else
                {
                    player.RadioTowerInsertedItemStartCinematic(packet.UniqueId);
                }
            }

            return true;
        }

        /**
         *
         * Radyo kulesi test modülü takılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnRadioTowerTOMUsing(RadioTowerTOMUsingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                RadioTowerProcessor.SendPacketToServer(ev.UniqueId, isTOMUsing: true);
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqued, bool isTOMUsing = false)
        {
            ServerModel.StoryRadioTowerArgs result = new ServerModel.StoryRadioTowerArgs()
            {
                UniqueId   = uniqued,
                IsTOMUsing = isTOMUsing
            };

            NetworkClient.SendPacket(result);
        }
    }
}

namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class TeleportationToolProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.TeleportationTool>();
            if (component == null)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player != null)
            {
                if (component.TeleporterId.IsNull())
                {
                    player.RightHandItemTransform.GetComponentInChildren<TeleportationTool>()?.beginTeleportVFX.Play();
                }
                else
                {
                    player.SeaTruckTeleportationStartCinematic(component.TeleporterId);
                }
            }

            return true;
        }

        /**
         *
         * Işınlanma işlemi başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTeleportationToolUsed(TeleportationToolUsedEventArgs ev)
        {
            ServerModel.PlayerItemActionArgs request = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.TeleportationTool()
                {
                    TeleporterId = ev.TeleporterId,
                }
            };

            NetworkClient.SendPacket(request);
        }
    }
}
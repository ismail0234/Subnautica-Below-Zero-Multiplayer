namespace Subnautica.Server.Processors.PDA
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class NotificationProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.NotificationAddedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsNotification)
            {
                profile.AddNotification(packet.Group, packet.Key, packet.IsAdded);
            }
            else if (packet.ColorIndex == -1)
            {
                profile.SetNotificationVisible(packet.Key, packet.IsVisible);
            }
            else 
            {
                profile.SetNotificationColorIndex(packet.Key, packet.ColorIndex);
            }

            return true;
        }
    }
}

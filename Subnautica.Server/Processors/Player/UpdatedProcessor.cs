namespace Subnautica.Server.Processors.Player
{
    using Server.Core;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class UpdatedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerUpdatedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.ItemInHand != TechType.None)
            {
                profile.AddUsedTool(packet.ItemInHand);
            }

            profile.SetPosition(packet.CompressedPosition.ToZeroVector3(), packet.CompressedRotation.ToZeroQuaternion());

            if (packet.CompressedLocalPosition != 0)
            {
                packet.CompressedPosition      = packet.CompressedLocalPosition;
                packet.CompressedLocalPosition = 0;
            }

            profile.SendPacketToOtherClients(packet, true);
            return true;
        }
    }
}

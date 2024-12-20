namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BaseMapRoomTransmissionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.BaseMapRoomTransmissionArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var baseDeconstructable = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(packet.UniqueId);
            if (baseDeconstructable == null)
            {
                return false;
            }

            var mapRoom = baseDeconstructable.GetMapRoomFunctionality();
            if (mapRoom == null)
            {
                return false;
            }

            mapRoom.gameObject.EnsureComponent<MultiplayerScannerRoom>()?.SetItems(packet.Items);
            return true;
        }
    }
}

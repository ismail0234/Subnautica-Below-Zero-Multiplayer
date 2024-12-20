namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class InteriorProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleInteriorArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsEntered)
            {
                profile.SetInterior(packet.VehicleId);
                
            }
            else
            {
                profile.SetInterior(null);
            }

            profile.SendPacketToOtherClients(packet);
            return true;
        }
    }
}

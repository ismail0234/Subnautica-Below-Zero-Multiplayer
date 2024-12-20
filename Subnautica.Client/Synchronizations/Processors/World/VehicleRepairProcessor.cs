namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class VehicleRepairProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleRepairArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var repair in packet.Repairs)
            {
                var liveMixin = Network.Identifier.GetComponentByGameObject<global::LiveMixin>(repair.VehicleId);
                if (liveMixin)
                {
                    ZeroLiveMixin.AddHealth(liveMixin, repair.Health);
                }
            }

            return true;
        }
    }
}

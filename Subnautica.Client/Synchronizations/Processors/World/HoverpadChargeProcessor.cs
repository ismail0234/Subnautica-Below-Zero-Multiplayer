namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.API.Features;

    using ServerModel = Subnautica.Network.Models.Server;

    public class HoverpadChargeProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.HoverpadChargeTransmissionArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var item in packet.Items)
            {
                if (Multiplayer.Constructing.Builder.TryGetBuildingValue(item.Key, out string uniqueId))
                {
                    var hoverpad = Network.Identifier.GetComponentByGameObject<global::Hoverpad>(uniqueId);
                    if (hoverpad == null || hoverpad.dockedBike == null)
                    {
                        continue;
                    }

                    var newHealth = (float) item.Value.Health;
                    var newCharge = (float) item.Value.Charge;

                    if (newHealth > hoverpad.dockedBike.liveMixin.health)
                    {
                        ZeroLiveMixin.AddHealth(hoverpad.dockedBike.liveMixin, newHealth);
                    }

                    if (newCharge > hoverpad.dockedBike.energyMixin.charge)
                    {
                        hoverpad.dockedBike.energyMixin.AddEnergy(newCharge - hoverpad.dockedBike.energyMixin.charge);
                    }  
                }
            }

            return true;
        }
    }
}

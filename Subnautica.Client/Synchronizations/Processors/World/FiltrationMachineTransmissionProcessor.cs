namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.API.Features;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FiltrationMachineTransmissionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.FiltrationMachineTransmissionArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var item in packet.TimeItems)
            {
                if (Multiplayer.Constructing.Builder.TryGetBuildingValue(item.ConstructionIndex, out string uniqueId))
                {
                    var geometry = Network.Identifier.GetComponentByGameObject<global::BaseFiltrationMachineGeometry>(uniqueId);
                    if (geometry == null)
                    {
                        continue;
                    }

                    var machine = geometry.GetModule();
                    if (machine == null)
                    {
                        continue;
                    }

                    machine.timeRemainingWater = item.TimeRemainingWater;
                    machine.timeRemainingSalt  = item.TimeRemainingSalt;
                }
            }

            return true;
        }
    }
}

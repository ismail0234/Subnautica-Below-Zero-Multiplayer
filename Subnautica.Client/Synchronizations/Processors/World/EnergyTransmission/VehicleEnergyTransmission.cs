namespace Subnautica.Client.Synchronizations.Processors.World.EnergyTransmission
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class VehicleEnergyTransmission : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleEnergyTransmissionArgs>();

            foreach (var powerCell in packet.PowerCells)
            {
                var vehicle = Network.Identifier.GetGameObject(powerCell.VehicleId);
                if (vehicle == null)
                {
                    continue;
                }

                this.ChargeVehicle(vehicle, CraftData.GetTechType(vehicle), powerCell.PowerCell1, powerCell.PowerCell2);
            }
          
            return true;
        }
        
        /**
         *
         * Araç enerjini senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ChargeVehicle(GameObject vehicle, TechType vehicleType, float powerCell1, float powerCell2)
        {
            if (vehicleType == TechType.Hoverbike)
            {
                if (powerCell1 != -1f)
                {
                    if (vehicle.TryGetComponent<global::Hoverbike>(out var hoverBike) && hoverBike.energyMixin.battery != null)
                    {
                        hoverBike.energyMixin.battery.charge = powerCell1;
                    }
                }
            }
            else if (vehicleType == TechType.Exosuit)
            {
                if (vehicle.TryGetComponent<global::Vehicle>(out var exosuit) && exosuit.energyInterface && exosuit.energyInterface.sources != null)
                {
                    if (powerCell1 != -1f && exosuit.energyInterface.sources.Length > 0 && exosuit.energyInterface.sources[0] && exosuit.energyInterface.sources[0].battery != null)
                    {
                        exosuit.energyInterface.sources[0].battery.charge = powerCell1;
                    }

                    if (powerCell2 != -1f && exosuit.energyInterface.sources.Length > 1 && exosuit.energyInterface.sources[1] && exosuit.energyInterface.sources[1].battery != null)
                    {
                        exosuit.energyInterface.sources[1].battery.charge = powerCell2;
                    }
                }
            }
            else if (vehicleType == TechType.SeaTruck)
            {
                foreach (var batterySource in vehicle.GetComponentsInChildren<global::BatterySource>())
                {
                    if (batterySource.name.Contains("Left"))
                    {
                        if (batterySource.battery != null)
                        {
                            batterySource.battery.charge = powerCell1;
                        }
                    }
                    else
                    {
                        if (batterySource.battery != null)
                        {
                            batterySource.battery.charge = powerCell2;
                        }
                    }
                }
            }
            else if (vehicleType == TechType.MapRoomCamera)
            {
                if (vehicle.TryGetComponent<MapRoomCamera>(out var mapRoomCamera))
                {
                    if (mapRoomCamera.energyMixin?.battery != null)
                    {
                        mapRoomCamera.energyMixin.battery.charge = powerCell1;
                    }
                }
            }
        }
    }
}

namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts;

    using UnityEngine;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class VehicleEnergyTransmission : BaseLogic
    {
        /**
         *
         * HoverbikePowerConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float HoverbikePowerConsumptionPerSecond { get; set; } = 0.07f;

        /**
         *
         * ExosuitPowerConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float ExosuitPowerConsumptionPerSecond { get; set; } = 0.09f;

        /**
         *
         * SeaTruckPowerConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float SeaTruckPowerConsumptionPerSecond { get; set; } = 0.12f;

        /**
         *
         * MapRoomCameraConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MapRoomCameraConsumptionPerSecond { get; set; } = 0.07f;

        /**
         *
         * ExosuitPowerConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float ExosuitJumpPowerConsumption { get; set; } = 1.2f;

        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * OldPositions nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, ZeroVector3> OldPositions { get; set; } = new Dictionary<string, ZeroVector3>();

        /**
         *
         * Requests nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<VehicleEnergyTransmissionItem> Requests { get; set; } = new List<VehicleEnergyTransmissionItem>();

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float deltaTime)
        {
            if (this.Timing.IsFinished() && API.Features.World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var vehicle in this.GetVehicles())
                {
                    if (!vehicle.IsUsingByPlayer)
                    {
                        this.ProcudeEnergy(vehicle, 1f, true);
                        continue;
                    }

                    if (!this.OldPositions.TryGetValue(vehicle.UniqueId, out var oldPosition))
                    {
                        this.OldPositions[vehicle.UniqueId] = oldPosition = vehicle.Position;
                    }

                    this.ProcudeEnergy(vehicle, 1f);
                    this.ConsumeEnergy(vehicle, oldPosition);
                    this.VehicleEnergyUpdateQueue(vehicle);

                    this.OldPositions[vehicle.UniqueId] = vehicle.Position;
                }

                this.SendPacketToAllClient();
            }
        }

        /**
         *
         * Araç enerjisi üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcudeEnergy(WorldDynamicEntity vehicle, float elapsedTime, bool autoSend = false)
        {
            if (vehicle.TechType != TechType.Exosuit)
            {
                return false;
            }

            var component = vehicle.Component.GetComponent<WorldEntityModel.Exosuit>();
            if (component.Modules.Count(q => q.ModuleType == TechType.ExosuitThermalReactorModule) <= 0)
            {
                return false;
            }

            if (component.PowerCells.Count(q => q.Charge != -1f) <= 0)
            {
                return false;
            }

            var exosuit = Network.Identifier.GetComponentByGameObject<global::Exosuit>(vehicle.UniqueId);
            if (exosuit)
            {
                var energyAmount = exosuit.thermalReactorCharge.Evaluate(exosuit.GetTemperature()) * elapsedTime;

                if (energyAmount > 0f)
                {
                    this.AddEnergy(component.PowerCells, energyAmount);

                    if (autoSend)
                    {
                        this.VehicleEnergyUpdateQueue(vehicle);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Araç enerjisi tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ConsumeEnergy(WorldDynamicEntity vehicle, ZeroVector3 oldPosition)
        {
            if (!Core.Server.Instance.Logices.PowerConsumer.IsTechnologyRequiresPower())
            {
                return true;
            }

            if (vehicle.TechType == TechType.Hoverbike)
            {
                if (vehicle.Position.Distance(oldPosition) > 2f)
                {
                    var component = vehicle.Component.GetComponent<WorldEntityModel.Hoverbike>();

                    component.Charge = Mathf.Max(0f, component.Charge - (this.HoverbikePowerConsumptionPerSecond * (component.IsLightActive ? 1.20f : 1f)));
                }
            }
            else if (vehicle.TechType == TechType.Exosuit)
            {
                if (vehicle.Position.Distance(oldPosition) > 0.75f)
                {
                    var component = vehicle.Component.GetComponent<WorldEntityModel.Exosuit>();

                    this.ConsumeEnergy(component.PowerCells, this.ExosuitPowerConsumptionPerSecond);                
                }
            }
            else if (vehicle.TechType == TechType.SeaTruck)
            {
                if (vehicle.Position.Distance(oldPosition) > 0.75f)
                {
                    var component = vehicle.Component.GetComponent<WorldEntityModel.SeaTruck>();
                    var powerRating = component.Modules.Any(q => q.ModuleType == TechType.SeaTruckUpgradeEnergyEfficiency) ? 0.8f : 1f;
                    
                    this.ConsumeEnergy(component.PowerCells, (this.SeaTruckPowerConsumptionPerSecond * powerRating) * (component.IsLightActive ? 1.20f : 1f));
                }
            }
            else if (vehicle.TechType == TechType.MapRoomCamera)
            {
                if (vehicle.Position.Distance(oldPosition) > 0.75f)
                {
                    var component = vehicle.Component.GetComponent<WorldEntityModel.MapRoomCamera>();

                    component.Battery.ConsumeEnergy(this.MapRoomCameraConsumptionPerSecond * (component.IsLightEnabled ? 1.20f : 1f), out var _);
                }
            }

            return true;
        }

        /**
         *
         * Mevcut enerji miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetEnergyAmount(List<PowerCell> powerCells)
        {
            float charge = 0f;

            for (int i = 0; i < powerCells.Count; i++)
            {
                charge += powerCells.ElementAt(i).Charge;
            }

            return charge;
        }

        /**
         *
         * Araç enerjisini tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConsumeEnergy(List<PowerCell> powerCells, float energyAmount)
        {
            var isConsumed = false;

            for (int i = 0; i < powerCells.Count; i++)
            {
                var charge = powerCells.ElementAt(i).Charge;
                if (charge > 0f && energyAmount > 0f)
                {
                    powerCells[i].Charge = Mathf.Max(0f, charge - energyAmount);

                    energyAmount -= charge - powerCells[i].Charge;

                    isConsumed = true;
                }
            }

            return isConsumed;
        }

        /**
         *
         * Araç enerjisini arttırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddEnergy(List<PowerCell> powerCells, float energyAmount)
        {
            for (int i = 0; i < powerCells.Count; i++)
            {
                var capacity = powerCells.ElementAt(i).Capacity;
                var charge   = powerCells.ElementAt(i).Charge;
                if (charge > 0f && energyAmount > 0f)
                {
                    powerCells[i].Charge = Mathf.Min(capacity, charge + energyAmount);

                    energyAmount -= powerCells[i].Charge - charge;
                }
            }
        }

        /**
         *
         * Araç enerjisini istek listesine alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void VehicleEnergyUpdateQueue(WorldDynamicEntity vehicle, bool autoSend = false)
        {
            if (vehicle.TechType == TechType.Hoverbike)
            {
                this.Requests.Add(new VehicleEnergyTransmissionItem(vehicle.UniqueId, vehicle.Component.GetComponent<WorldEntityModel.Hoverbike>().Charge, 0f));
            }
            else if (vehicle.TechType == TechType.Exosuit)
            {
                var component = vehicle.Component.GetComponent<WorldEntityModel.Exosuit>();

                this.Requests.Add(new VehicleEnergyTransmissionItem(vehicle.UniqueId, component.PowerCells.ElementAt(0).Charge, component.PowerCells.ElementAt(1).Charge));
            }
            else if (vehicle.TechType == TechType.SeaTruck)
            {
                var component = vehicle.Component.GetComponent<WorldEntityModel.SeaTruck>();

                this.Requests.Add(new VehicleEnergyTransmissionItem(vehicle.UniqueId, component.PowerCells.ElementAt(0).Charge, component.PowerCells.ElementAt(1).Charge));
            }
            else if (vehicle.TechType == TechType.MapRoomCamera)
            {
                this.Requests.Add(new VehicleEnergyTransmissionItem(vehicle.UniqueId, vehicle.Component.GetComponent<WorldEntityModel.MapRoomCamera>().Battery.Charge, 0f));
            }

            if (autoSend)
            {
                this.SendPacketToAllClient();
            }
        }

        /**
         *
         * Yakındaki oyunculara verileri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendPacketToAllClient()
        {
            if (this.Requests.Count > 0)
            {
                var request = new ServerModel.VehicleEnergyTransmissionArgs()
                {
                    PowerCells = this.Requests.ToList()
                };

                if (request.PowerCells.Any())
                {
                    Core.Server.SendPacketToAllClient(request);
                }

                this.Requests.Clear();
            }
        }

        /**
         *
         * Hoverpad'leri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity[] GetVehicles()
        {
            return Core.Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType.IsVehicle(true, false)).ToArray();
        }
    }
}

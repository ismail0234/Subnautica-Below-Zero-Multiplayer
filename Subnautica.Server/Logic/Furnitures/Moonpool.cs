namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;

    public class Moonpool : BaseLogic
    {
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
         * Tamir edilecek sağlık (saniye başına)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float RepairHealth { get; set; } = 12.5f;

        /**
         *
         * Requests nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<ServerModel.VehicleRepairItem> Requests { get; set; } = new List<ServerModel.VehicleRepairItem>();

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var construction in this.GetMoonpools())
                {
                    var component = construction.Value.EnsureComponent<Metadata.BaseMoonpool>();
                    if (component.IsDocked)
                    {
                        var dockingBay = Network.Identifier.GetComponentByGameObject<global::VehicleDockingBay>(construction.Value.UniqueId);
                        if (dockingBay == null)
                        {
                            continue;
                        }

                        bool isCharged = false;

                        if (component.Vehicle.TechType == TechType.SeaTruck)
                        {
                            isCharged = this.ChargeVehicle(dockingBay, component.Vehicle.Component.GetComponent<WorldEntityModel.SeaTruck>().PowerCells);
                        }

                        if (component.Vehicle.TechType == TechType.Exosuit)
                        {
                            isCharged = this.ChargeVehicle(dockingBay, component.Vehicle.Component.GetComponent<WorldEntityModel.Exosuit>().PowerCells);
                        }

                        if (isCharged)
                        {
                            Server.Core.Server.Instance.Logices.VehicleEnergyTransmission.VehicleEnergyUpdateQueue(component.Vehicle);
                        }

                        if (construction.Value.TechType == TechType.BaseMoonpoolExpansion)
                        {
                            this.RepairVehicle(component.Vehicle.UniqueId, component.Vehicle.Component.GetComponent<WorldEntityModel.SeaTruck>());
                        }
                    }
                }

                this.SendPacketToAllClient();
            }
        }

        /**
         *
         * Tamir paketini oyunculara gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendPacketToAllClient()
        {
            if (this.Requests.Count > 0)
            {
                ServerModel.VehicleRepairArgs request = new ServerModel.VehicleRepairArgs()
                {
                    Repairs = this.Requests.ToList()
                };

                Core.Server.SendPacketToAllClient(request);

                this.Requests.Clear();
            }
        }

        /**
         *
         * Aracı tamir eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool RepairVehicle(string vehicleId, WorldEntityModel.SeaTruck seatruck)
        {
            if (seatruck.LiveMixin.AddHealth(this.RepairHealth))
            {
                this.Requests.Add(new ServerModel.VehicleRepairItem(vehicleId, seatruck.LiveMixin.Health));
                return true;
            }

            return false;
        }

        /**
         *
         * Aracı şarj eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ChargeVehicle(global::VehicleDockingBay dockingBay, List<PowerCell> powerCells)
        {
            var energyAmount = this.GetEnergyValue(powerCells);
            if (energyAmount <= 0f)
            {
                return false;
            }

            var isCharged = false;

            foreach (var powerCell in powerCells)
            {
                if (powerCell.IsExists && !powerCell.IsFull && energyAmount > 0f)
                {
                    if (Core.Server.Instance.Logices.PowerConsumer.HasPower(dockingBay.powerRelay, energyAmount))
                    {
                        if (powerCell.AddEnergy(energyAmount, out var addedEnergyAmount))
                        {
                            Core.Server.Instance.Logices.PowerConsumer.ConsumePower(dockingBay.powerRelay, addedEnergyAmount, out var _);

                            energyAmount -= addedEnergyAmount;

                            isCharged = true;
                        }
                    }
                }
            }

            return isCharged;
        }

        /**
         *
         * Üretilen enerji miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetEnergyValue(List<PowerCell> powerCells)
        {
            var capacity = 0f;
            foreach (var powerCell in powerCells)
            {
                if (powerCell.IsExists)
                {
                    capacity += powerCell.Capacity;
                }
            }

            if (capacity == 0f)
            {
                return 0f;
            }

            return capacity * (1f / 400f);
        }

        /**
         *
         * Hoverpad'leri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<KeyValuePair<string, ConstructionItem>> GetMoonpools()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.ConstructedAmount == 1f && (q.Value.TechType == TechType.BaseMoonpool || q.Value.TechType == TechType.BaseMoonpoolExpansion)).ToList();
        }
    }
}

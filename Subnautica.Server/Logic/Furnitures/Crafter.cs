namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Extensions;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class Crafter : BaseLogic
    {
        /**
         *
         * Bekleyen İşlemler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Queue<MetadataComponentArgs> Queue { get; set; } = new Queue<MetadataComponentArgs>();

        /**
         *
         * Tüketilecek enerji miktarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float EnergyAmount { get; set; } = 5f;

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (World.IsLoaded && this.Queue.Count > 0)
            {
                while (this.Queue.Count > 0)
                {
                    var packet = this.Queue.Dequeue();

                    if (Core.Server.Instance.Logices.World.IsStaticFabricator(packet.UniqueId))
                    {
                        Core.Server.SendPacketToAllClient(packet);
                        continue;
                    }

                    var ghostCrafter = Network.Identifier.GetGameObject(packet.UniqueId)?.GetGhostCrafter();
                    if (ghostCrafter == null)
                    {
                        continue;
                    }

                    if (ghostCrafter.baseComp && !ghostCrafter.baseComp.IsPowered(ghostCrafter.transform.position))
                    {
                        continue;
                    }

                    if (ghostCrafter.needsPower)
                    {
                        if (Core.Server.Instance.Logices.PowerConsumer.IsTechnologyRequiresPower() && ghostCrafter.powerRelay.GetPower() < this.EnergyAmount)
                        {
                            continue;
                        }
                    }

                    var worldEntity = this.GetSeaTruckEntity(packet.UniqueId);
                    if (worldEntity != null)
                    {
                        if (Core.Server.Instance.Logices.PowerConsumer.IsTechnologyRequiresPower())
                        {
                            var seaTruckComponent = worldEntity.Component.GetComponent<WorldEntityModel.SeaTruck>();
                            if (seaTruckComponent == null)
                            {
                                continue;
                            }

                            if (Core.Server.Instance.Logices.VehicleEnergyTransmission.GetEnergyAmount(seaTruckComponent.PowerCells) < this.EnergyAmount)
                            {
                                continue;
                            }

                            Core.Server.Instance.Logices.VehicleEnergyTransmission.ConsumeEnergy(seaTruckComponent.PowerCells, this.EnergyAmount);
                            Core.Server.Instance.Logices.VehicleEnergyTransmission.VehicleEnergyUpdateQueue(worldEntity, true);
                        }
                    }
                    else if (ghostCrafter.needsPower)
                    {
                        Core.Server.Instance.Logices.PowerConsumer.ConsumePower(ghostCrafter.powerRelay, this.EnergyAmount, out var ____);
                    }

                    Core.Server.SendPacketToAllClient(packet);
                }
            }
        }

        /**
         *
         * Craft işlemini kuyruğa alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Craft(MetadataComponentArgs packet)
        {
            this.Queue.Enqueue(packet);
        }

        /**
         *
         * SeaTruck Crafter Nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity GetSeaTruckEntity(string uniqueId) 
        {
            foreach (var module in Core.Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckFabricatorModule))
            {
                var component = module.Component.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>();
                if (component != null && component.FabricatorUniqueId == uniqueId)
                {
                    var seaTruck = module.GetSeaTruckHeadModule();
                    if (seaTruck != null && seaTruck.TechType == TechType.SeaTruck)
                    {
                        return seaTruck;
                    }
                }
            }

            return null;
        }
    }
}

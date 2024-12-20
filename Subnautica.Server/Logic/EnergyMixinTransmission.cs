namespace Subnautica.Server.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class EnergyMixinTransmission : BaseLogic
    {
        /**
         *
         * ThumperEnergyConsumptionPerSecond nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float ThumperEnergyConsumptionPerSecond { get; set; } = 0.125f;

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
         * Requests nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<EnergyMixinTransmissionItem> Requests { get; set; } = new List<EnergyMixinTransmissionItem>();

        /**
         *
         * Her sabit tick'den sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished() && API.Features.World.IsLoaded)
            {
                this.Timing.Restart();

                if (Core.Server.Instance.Logices.PowerConsumer.IsTechnologyRequiresPower())
                {
                    foreach (var item in this.GetItems())
                    {
                        if (item.Component == null)
                        {
                            continue;
                        }

                        var energyAmount = this.ConsumeEnergy(item);
                        if (energyAmount != -1f)
                        {
                            this.Requests.Add(new EnergyMixinTransmissionItem(item.Id, energyAmount, item.Position));
                        }
                    }

                    this.SendPacketToAllClient();
                }
            }
        }

        /**
         *
         * Enerji tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float ConsumeEnergy(WorldDynamicEntity item)
        {
            if (item.TechType == TechType.Thumper)
            {
                var component = item.Component.GetComponent<WorldEntityModel.Thumper>();
                if (component.Charge == -1f)
                {
                    return -1f;
                }

                if (component.Charge > 0f)
                {
                    component.Charge = Math.Max(0f, component.Charge - this.ThumperEnergyConsumptionPerSecond);
                }

                return component.Charge;
            }
            else if (item.TechType == TechType.Flare)
            {
                var component = item.Component.GetComponent<WorldEntityModel.Flare>();
                component.Energy = Math.Max(0f, component.Energy - this.GetWindBurnDownScalar());

                if (component.Energy <= 0f)
                {
                    Core.Server.Instance.Storages.World.RemoveDynamicEntity(item.UniqueId);

                    this.SendSinglePacketToAllClient(new EnergyMixinTransmissionItem(item.Id, 0f, item.Position));
                    return -1f;
                }

                return component.Energy;
            }

            return -1f;
        }

        /**
         *
         * Rüzgar gücünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetWindBurnDownScalar()
        {
            return (float) Tools.GetRandomInt(1, 6);
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
                foreach (var profile in Core.Server.Instance.GetPlayers())
                {
                    var request = new ServerModel.EnergyMixinTransmissionArgs()
                    {
                        Items = new List<EnergyMixinTransmissionItem>()
                    };

                    foreach (var item in this.Requests)
                    {
                        var distance = item.Position.Distance(profile.Position);
                        if (distance < 400f || (item.Charge <= 0f && distance < 1600f))
                        {
                            request.Items.Add(item);
                        }
                    }

                    if (request.Items.Any())
                    {
                        profile.SendPacket(request);
                    }
                }

                this.Requests.Clear();
            }
        }

        /**
         *
         * Tüm oyunculara verileri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendSinglePacketToAllClient(EnergyMixinTransmissionItem item)
        {
            var packet = new ServerModel.EnergyMixinTransmissionArgs()
            {
                Items = new List<EnergyMixinTransmissionItem>()
            };

            packet.Items.Add(item);

            Core.Server.SendPacketToAllClient(packet);
        }

        /**
         *
         * Nesneleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WorldDynamicEntity[] GetItems()
        {
            return Core.Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.Thumper || q.TechType == TechType.Flare).ToArray();
        }
    }
}

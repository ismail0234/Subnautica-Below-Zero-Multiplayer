namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts;

    using UnityEngine;

    using WorldChildrens = Subnautica.Network.Models.Storage.World.Childrens;
    using Metadata       = Subnautica.Network.Models.Metadata;
    using ServerModel    = Subnautica.Network.Models.Server;

    public class EnergyTransmission : BaseLogic
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
         * Her tick'de tetiklenir.
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
                    this.GenerateEnergy();
                    this.SendEnergyToAllClients();
                }
            }
        }

        /**
         *
         * Enerji üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void GenerateEnergy()
        {
            var constructions = this.GetEnergyConstructions();

            foreach (WorldChildrens.PowerSource powerSource in this.GetPowerSources())
            {
                var construction = constructions.Where(q => q.Value.Id == powerSource.ConstructionId).FirstOrDefault();
                if (construction.Value == null)
                {
                    // Core.Server.Instance.Storages.World.Storage.PowerSources.Remove(powerSource);
                    continue;
                }

                if (powerSource.Power >= powerSource.MaxPower)
                {
                    continue;
                }

                if (construction.Value.TechType == TechType.SolarPanel)
                {
                    this.GenerateSolarPanelEnergy(Network.Identifier.GetComponentByGameObject<global::SolarPanel>(construction.Value.UniqueId), powerSource);
                }
                else if (construction.Value.TechType == TechType.ThermalPlant)
                {
                    this.GenerateThermalPlantEnergy(Network.Identifier.GetComponentByGameObject<global::ThermalPlant>(construction.Value.UniqueId), powerSource);
                }
                else if (construction.Value.TechType == TechType.BaseNuclearReactor)
                {
                    this.GenerateNuclearReactorEnergy(Network.Identifier.GetComponentByGameObject<global::BaseNuclearReactorGeometry>(construction.Value.UniqueId).GetModule(), powerSource, construction.Value);
                }
                else if (construction.Value.TechType == TechType.BaseBioReactor)
                {
                    this.GenerateBioReactorReactorEnergy(Network.Identifier.GetComponentByGameObject<global::BaseBioReactorGeometry>(construction.Value.UniqueId).GetModule(), powerSource, construction.Value);
                }
            }
        }


        /**
         *
         * Güneş paneli enerjilerini üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool GenerateSolarPanelEnergy(global::SolarPanel solarPanel, WorldChildrens.PowerSource powerSource, float elapsedTime = 1f)
        {
            powerSource.ModifyPower(solarPanel.GetRechargeScalar() * elapsedTime * 0.25f * 5.0f);
            return true;
        }

        /**
         *
         * Thermal ısı enerjilerini üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool GenerateThermalPlantEnergy(global::ThermalPlant thermalPlant, WorldChildrens.PowerSource powerSource, float elapsedTime = 1f)
        {
            if (thermalPlant.temperature <= 25f)
            {
                return false;
            }

            powerSource.ModifyPower(1.65f * (elapsedTime * Mathf.Clamp01(Mathf.InverseLerp(25f, 100f, thermalPlant.temperature))));
            return true;
        }

        /**
         *
         * Biyo Reaktör enerjilerini üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool GenerateBioReactorReactorEnergy(global::BaseBioReactor bioReactor, WorldChildrens.PowerSource powerSource, ConstructionItem construction, float elapsedTime = 1f)
        {
            if (!bioReactor.producingPower)
            {
                return false;
            }

            float requested  = 0.8333333f * elapsedTime;
            float leftEnergy = powerSource.MaxPower - powerSource.Power;
            if (leftEnergy < requested)
            {
                requested = leftEnergy; 
            }

            powerSource.ModifyPower(this.CalculateBioReactorProducePower(requested, bioReactor, powerSource, construction));
            return true;
        }

        /**
         *
         * Nükleer Reaktör enerjilerini üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool GenerateNuclearReactorEnergy(global::BaseNuclearReactor nuclearReactor, WorldChildrens.PowerSource powerSource, ConstructionItem construction, float elapsedTime = 1f)
        {
            if (!nuclearReactor.producingPower)
            {
                return false;
            }

            float requested  = 4.166667f * elapsedTime;
            float leftEnergy = powerSource.MaxPower - powerSource.Power;
            if (leftEnergy < requested)
            {
                requested = leftEnergy;
            }

            powerSource.ModifyPower(this.CalculateNuclearReactorProducePower(requested, nuclearReactor, powerSource, construction));
            return true;
        }

        /**
         *
         * Nükleer Reaktör üretilecek enerji miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float CalculateNuclearReactorProducePower(float requested, global::BaseNuclearReactor nuclearReactor, WorldChildrens.PowerSource powerSource, ConstructionItem construction)
        {
            var component = construction.Component?.GetComponent<Metadata.NuclearReactor>();
            if (requested <= 0.0f || component == null || component.Items.Count <= 0)
            {
                return 0.0f;
            }

            powerSource.ModifyConsumedEnergy(requested);

            int consumedReactorRod = 0, removedRod = 0;

            for (int index = 0; index < component.Items.Count; index++)
            {
                if (global::BaseNuclearReactor.charge.TryGetValue(component.Items.ElementAt(index), out float maxPower))
                {
                    if (powerSource.ConsumedEnergy < maxPower)
                    {
                        consumedReactorRod++;
                    }
                    else
                    {
                        powerSource.ModifyConsumedEnergy(-maxPower);
                        component.Items[index] = TechType.DepletedReactorRod;

                        removedRod++;
                    }
                }
            }

            if (removedRod > 0)
            {
                this.SendRemoveNuclearReactorItems(construction.UniqueId, component.Items);
            }

            if (consumedReactorRod == 0)
            {
                requested -= powerSource.ConsumedEnergy;
                powerSource.SetConsumedEnergy(0.0f);
            }

            return requested;
        }

        /**
         *
         * Biyo Reaktör üretilecek enerji miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float CalculateBioReactorProducePower(float requested, global::BaseBioReactor bioReactor, WorldChildrens.PowerSource powerSource, ConstructionItem construction)
        {
            var component = construction.EnsureComponent<Metadata.BioReactor>();
            if (requested <= 0.0f || component == null)
            {
                return 0.0f;
            }

            var totalItem = component.StorageContainer.Items.Count;
            if (totalItem <= 0)
            {
                return 0.0f;
            }

            powerSource.ModifyConsumedEnergy(requested);

            var removingItems = new List<WorldPickupItem>();

            foreach (var item in component.StorageContainer.Items.ToList())
            {
                if (global::BaseBioReactor.charge.TryGetValue(item.TechType, out float maxPower) && powerSource.ConsumedEnergy >= maxPower)
                {
                    powerSource.ModifyConsumedEnergy(-maxPower);

                    component.StorageContainer.RemoveItem(item);

                    removingItems.Add(WorldPickupItem.Create(item, PickupSourceType.StorageContainer));
                }
            }

            if (removingItems.Count > 0)
            {
                this.SendRemoveBioReactorItems(construction.UniqueId, removingItems);
            }

            if (component.StorageContainer.Items.Count == 0)
            {
                requested -= powerSource.ConsumedEnergy;
                powerSource.SetConsumedEnergy(0.0f);
            }
    
            return requested;
        }

        /**
         *
         * Biyo Reaktör eşyalarını kaldırmak için istek gönderirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendRemoveBioReactorItems(string uniqueId, List<WorldPickupItem> items)
        {
            foreach (var pickupItem in items)
            {
                ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
                {
                    UniqueId  = uniqueId,
                    TechType  = TechType.BaseBioReactor,
                    Component = new Metadata.BioReactor()
                    {
                        IsAdded = false,
                        WorldPickupItem = pickupItem,
                    },
                };

                Server.Core.Server.SendPacketToAllClient(request);
            }
        }

        /**
         *
         * Nükleer Reaktör eşyalarını kaldırmak için istek gönderirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendRemoveNuclearReactorItems(string uniqueId, List<TechType> items)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                TechType  = TechType.BaseNuclearReactor,
                Component = new Metadata.NuclearReactor()
                {
                    IsRemoving = true,
                    Items      = items,
                },
            };

            Server.Core.Server.SendPacketToAllClient(result);
        }

        /**
         *
         * Yapı tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnConstructionComplete(ConstructionItem construction)
        {
            if (!TechGroup.EnergyConstructions.Contains(construction.TechType))
            {
                return false;
            }

            if (Core.Server.Instance.Storages.World.Storage.PowerSources.Where(q => q.ConstructionId == construction.Id).Any())
            {
                return false;
            }

            var powerSource = new WorldChildrens.PowerSource()
            {
                ConstructionId = construction.Id,
                MaxPower       = this.GetMaxPower(construction.TechType),
            };

            Core.Server.Instance.Storages.World.Storage.PowerSources.Add(powerSource);
            return true;
        }

        /**
         *
         * Yapı yıkıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnConstructionRemove(uint constructionId, string uniqueId)
        {
            Core.Server.Instance.Storages.World.Storage.PowerSources.RemoveWhere(q => q.ConstructionId == constructionId);
            return true;
        }

        /**
         *
         * Max gücü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetMaxPower(TechType techType)
        {
            switch (techType)
            {
                case TechType.SolarPanel: return 75f;
                case TechType.ThermalPlant: return 250f;
                case TechType.BaseBioReactor: return 500f;
                case TechType.BaseNuclearReactor: return 2500f;
            }

            return 1f;
        }

        /**
         *
         * Enerjileri tüm oyunculara gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendEnergyToAllClients()
        {
            var constructions = this.GetEnergyConstructions();

            foreach (var powerSources in this.GetPowerSources().Split(125))
            {
                EnergyTransmissionArgs request = new EnergyTransmissionArgs()
                {
                    PowerSources = new Dictionary<uint, float>()
                };

                foreach (var item in powerSources)
                {
                    var construction = constructions.FirstOrDefault(q => q.Value.Id == item.ConstructionId);
                    if (construction.Value != null)
                    {
                        request.PowerSources.Add(construction.Value.Id, item.Power);
                    }
                }

                if (request.PowerSources.Any())
                {
                    Core.Server.SendPacketToAllClient(request);
                }
            }
        }

        /**
         *
         * Enerji yapılarının bilgilerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WorldChildrens.PowerSource[] GetPowerSources()
        {
            return Core.Server.Instance.Storages.World.Storage.PowerSources.ToArray();
        }

        /**
         *
         * Enerji sağlayan yapıları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetEnergyConstructions()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => TechGroup.EnergyConstructions.Contains(q.Value.TechType) && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}

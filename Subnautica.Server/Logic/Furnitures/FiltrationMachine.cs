namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class FiltrationMachine : BaseLogic
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
         * MaxWater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte MaxWater { get; set; } = 2;

        /**
         *
         * MaxSalt değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte MaxSalt { get; set; } = 2;

        /**
         *
         * Saniyede tüketilen enerji miktaını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float EnergyRequiredPerSecond { get; set; } = 0.85f;

        /**
         *
         * ElapsedTime
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float ElapsedTime { get; set; } = 1f;

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float deltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                List<FiltrationMachineTimeItem> timeRemainings = new List<FiltrationMachineTimeItem>();

                foreach (var construction in this.GetFiltrationMachines())
                {
                    var component = construction.Value.EnsureComponent<Metadata.FiltrationMachine>();
                    if (!component.IsUnderwater)
                    {
                        continue;
                    }

                    var items = component.Items.Where(q => q.ItemId.IsNull());
                    if (items.Count() <= 0)
                    {
                        continue;
                    }
                    
                    var geometry = Network.Identifier.GetComponentByGameObject<global::BaseFiltrationMachineGeometry>(construction.Value.UniqueId);
                    if (geometry == null)
                    {
                        continue;
                    }

                    var machine = geometry.GetModule();
                    if (machine == null)
                    {
                        continue;
                    }

                    if (!machine.IsUnderwater())
                    {
                        component.IsUnderwater = false;
                        continue;
                    }

                    if (component.TimeRemainingWater > 0.0f || component.TimeRemainingSalt > 0.0f)
                    {
                        if (machine.IsPowered() && Core.Server.Instance.Logices.PowerConsumer.HasPower(machine.powerConsumer, this.EnergyRequiredPerSecond))
                        {
                            if (!Core.Server.Instance.Logices.PowerConsumer.ConsumePower(machine.powerConsumer, this.EnergyRequiredPerSecond, out var _))
                            {
                                continue;
                            }

                            bool isUpdated = false;

                            if (component.TimeRemainingWater > 0.0f)
                            {
                                component.TimeRemainingWater = Mathf.Max(0.0f, component.TimeRemainingWater - this.ElapsedTime);

                                if (component.TimeRemainingWater == 0.0f)
                                {
                                    component.TimeRemainingWater = -1f;

                                    if (this.Spawn(construction.Value.UniqueId, component, TechType.BigFilteredWater))
                                    {
                                        this.TryFilterWater(component);
                                    }
                                }
                                else
                                {
                                    isUpdated = true;
                                }
                            }

                            if (component.TimeRemainingSalt > 0.0f)
                            {
                                component.TimeRemainingSalt = Mathf.Max(0.0f, component.TimeRemainingSalt - this.ElapsedTime);

                                if (component.TimeRemainingSalt == 0.0f)
                                {
                                    component.TimeRemainingSalt = -1f;

                                    if (this.Spawn(construction.Value.UniqueId, component, TechType.Salt))
                                    {
                                        this.TryFilterSalt(component);
                                    }
                                }
                                else
                                {
                                    isUpdated = true;
                                }
                            }

                            if (isUpdated)
                            {
                                timeRemainings.Add(new FiltrationMachineTimeItem(construction.Value.Id, component.TimeRemainingWater, component.TimeRemainingSalt, construction.Value.CellPosition));
                            }
                        }
                    }
                }

                if (timeRemainings.Count > 0)
                {
                    this.SendTimeRemaining(timeRemainings);
                }
            }
        }

        /**
         *
         * Su üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryFilterWater(Metadata.FiltrationMachine component)
        {
            if (component.TimeRemainingWater > 0.0f || component.Items.Where(q => !string.IsNullOrEmpty(q.ItemId) && q.TechType == TechType.BigFilteredWater).Count() >= this.MaxWater)
            {
                return false;
            }

            component.TimeRemainingWater = 840f;
            return true;
        }

        /**
         *
         * Tuz üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryFilterSalt(Metadata.FiltrationMachine component)
        {
            if (component.TimeRemainingSalt > 0.0f || component.Items.Where(q => !string.IsNullOrEmpty(q.ItemId) && q.TechType == TechType.Salt).Count() >= this.MaxSalt)
            {
                return false;
            }

            component.TimeRemainingSalt = 420f;
            return true;
        }

        /**
         *
         * Tüm kullanıcılara paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool Spawn(string uniqueId, Metadata.FiltrationMachine component, TechType techType)
        {
            var item = component.Items.Where(q => string.IsNullOrEmpty(q.ItemId) && q.TechType == techType).FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            item.ItemId = Network.Identifier.GenerateUniqueId();

            ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                TechType  = TechType.BaseFiltrationMachine,
                Component = new Metadata.FiltrationMachine()
                {
                    TimeRemainingWater = component.TimeRemainingWater,
                    TimeRemainingSalt  = component.TimeRemainingSalt,

                    Item = new Metadata.FiltrationMachineItem()
                    {
                        TechType = techType,
                        ItemId   = item.ItemId,
                    }
                },
            };

            Core.Server.SendPacketToAllClient(request);
            return true;
        }

        /**
         *
         * Üretim zamanını kullanıcılara iletir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendTimeRemaining(List<FiltrationMachineTimeItem> timeItems)
        {
            foreach (var profile in Core.Server.Instance.GetPlayers())
            {
                var requestItems = new List<Metadata.FiltrationMachineTimeItem>();

                foreach (var timeItem in timeItems)
                {
                    if (timeItem.Position.Distance(profile.Position) <= 100f)
                    {
                        requestItems.Add(timeItem);
                    }
                }

                if (requestItems.Any())
                {
                    foreach (var _requestItems in requestItems.Split(30))
                    {
                        FiltrationMachineTransmissionArgs request = new FiltrationMachineTransmissionArgs()
                        {
                            TimeItems = _requestItems.ToList()
                        };

                        profile.SendPacket(request);
                    }
                }
            }
        }

        /**
         *
         * Kahve makinelerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetFiltrationMachines()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseFiltrationMachine && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}

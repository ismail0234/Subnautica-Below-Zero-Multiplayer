namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class BatteryCharger : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(3000f);

        /**
         *
         * Her sabit tick'de tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                this.BulkChargeTheBatteries(3f);
            }
        }

        /**
         *
         * Toplu Bataryaları şarj eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void BulkChargeTheBatteries(float elapsedTime)
        {
            var chargers = new List<Metadata.ChargerSimple>();

            foreach (var construction in this.GetBatteryChargerConstructions())
            {
                var chargerSimple = this.GetSingleChargeTheBatteries(construction.Value, elapsedTime);
                if (chargerSimple != null)
                {
                    chargers.Add(chargerSimple);
                }
            }

            if (chargers.Count > 0)
            {
                this.SendBatteryToAllClients(chargers);
            }
        }

        /**
         *
         * Bataryaları şarj eder ve döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Metadata.ChargerSimple GetSingleChargeTheBatteries(ConstructionItem construction, float elapsedTime)
        {
            var charger = Network.Identifier.GetComponentByGameObject<global::Charger>(construction.UniqueId);
            if (charger == null)
            {
                return null;
            }

            var component = construction.EnsureComponent<Metadata.Charger>();
            if (!component.Items.Where(q => q.IsActive).Any())
            {
                return null;
            }

            Metadata.ChargerSimple chargerSimple = new Metadata.ChargerSimple()
            {
                ConstructionId = construction.Id,
                Position       = construction.PlacePosition,
                Batteries      = new float[TechGroup.GetBatterySlotAmount(construction.TechType)],
                IsPowered      = false,
                IsCharging     = false,
            };

            int totalBattery = 0;

            if (charger.powerConsumer.IsPowered())
            {
                float requiredPower = 0.0f;
                foreach (BatteryItem battery in component.Items.Where(q => q.IsActive))
                {
                    if (battery.Charge < battery.Capacity)
                    {
                        float power = elapsedTime * charger.chargeSpeed * battery.Capacity;
                        if (battery.Charge + power > battery.Capacity)
                        {
                            power = battery.Capacity - battery.Charge;
                        }

                        requiredPower += power;
                        totalBattery++;
                    }
                }

                float consumedPower = 0.0f;
                if (requiredPower > 0.0f && Core.Server.Instance.Logices.PowerConsumer.HasPower(charger.powerConsumer, requiredPower))
                {
                    chargerSimple.IsPowered = true;

                    Core.Server.Instance.Logices.PowerConsumer.ConsumePower(charger.powerConsumer, requiredPower, out consumedPower);
                }

                if (consumedPower > 0.0f)
                {
                    chargerSimple.IsCharging = true;

                    float averageEnergy = consumedPower / (float) totalBattery;
    
                    for(int i = 0; i < component.Items.Count; i++)
                    {
                        BatteryItem battery = component.Items.ElementAt(i);
                        if (battery.IsActive)
                        {
                            if (battery.Charge < battery.Capacity)
                            {
                                float power    = averageEnergy;
                                float maxPower = battery.Capacity - battery.Charge;
                                if (power > maxPower)
                                {
                                    power = maxPower;
                                }

                                battery.Charge += power;
                            }
                        }
                    }
                }
            }

            chargerSimple.IsPowered = totalBattery == 0 | chargerSimple.IsPowered;

            foreach (BatteryItem battery in component.Items.Where(q => q.IsActive))
            {
                chargerSimple.Batteries[battery.GetSlotId() - 1] = battery.Charge;
            }

            return chargerSimple;
        }

        /**
         *
         * Enerjileri tüm oyunculara gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendBatteryToAllClients(List<Metadata.ChargerSimple> chargers)
        {
            foreach (var profile in Core.Server.Instance.GetPlayers())
            {
                var customChargers = new List<Metadata.ChargerSimple>();

                for (int i = 0; i < chargers.Count; i++)
                {
                    var charger = chargers.ElementAt(i);
                    if (charger.Position.Distance(profile.Position) <= 350)
                    {
                        customChargers.Add(charger);
                    }
                }

                if (customChargers.Any())
                {
                    foreach (var _customChargers in customChargers.Split(30))
                    {
                        BatteryChargerTransmissionArgs request = new BatteryChargerTransmissionArgs()
                        {
                            Chargers = _customChargers.ToList()
                        };

                        profile.SendPacket(request);
                    }

                    customChargers.Clear();
                }
            }

            chargers.Clear();
        }

        /**
         *
         * Enerji sağlayan yapıları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetBatteryChargerConstructions()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => TechGroup.BatteryChargers.Contains(q.Value.TechType) && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}
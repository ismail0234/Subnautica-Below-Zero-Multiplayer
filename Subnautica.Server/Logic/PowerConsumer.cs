namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;
    using UnityEngine;

    public class PowerConsumer : BaseLogic
    {
        /**
         *
         * Güç kaynaklarını önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, HashSet<string>> PowerRelays { get; set; } = new Dictionary<string, HashSet<string>>();

        /**
         *
         * Relay içerisine güç kaynağı eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPowerSourceAdding(PowerSourceAddingEventArgs ev)
        {
            if (!this.PowerRelays.TryGetValue(ev.UniqueId, out var powerSources))
            {
                this.PowerRelays[ev.UniqueId] = new HashSet<string>();
            }

            if (API.Features.World.IsLoaded)
            {
                this.AddPowerSourceToCache(ev.UniqueId, ev.PowerSource);
            }
        }

        /**
         *
         * Relay içerisinden güç kaynağı kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPowerSourceRemoving(PowerSourceRemovingEventArgs ev)
        {
            if (API.Features.World.IsLoaded)
            {
                if (this.PowerRelays.TryGetValue(ev.UniqueId, out var powerSources))
                {
                    var powerSourceId = this.GetPowerSourceId(ev.PowerSource.GetGameObject());
                    if (powerSourceId.IsNotNull())
                    {
                        powerSources.Remove(powerSourceId);
                    }
                }
            }
        }

        /**
         *
         * Oyun başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGameStart()
        {
            foreach (var powerRelay in this.PowerRelays)
            {
                var relay = Network.Identifier.GetComponentByGameObject<global::PowerRelay>(powerRelay.Key);
                if (relay.inboundPowerSources.Count > 0)
                {
                    foreach (var powerSource in relay.inboundPowerSources)
                    {
                        this.AddPowerSourceToCache(powerRelay.Key, powerSource);
                    }
                }

                if (relay.internalPowerSource != null)
                {
                    this.AddPowerSourceToCache(powerRelay.Key, relay.internalPowerSource);
                }
            }
        }

        /**
         *
         * Güç kaynaığını önbelleğe ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void AddPowerSourceToCache(string relayId, IPowerInterface powerSource)
        {
            var powerSourceId = this.GetPowerSourceId(powerSource.GetGameObject());
            if (powerSourceId.IsNotNull())
            {
                if (!this.PowerRelays[relayId].Contains(powerSourceId))
                {
                    this.PowerRelays[relayId].Add(powerSourceId);
                }
            }
        }

        /**
         *
         * Enerji gerekli mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTechnologyRequiresPower()
        {
            return GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower);
        }

        /**
         *
         * Yeterli güç olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasPower(global::PowerConsumer powerConsumer, float power)
        {
            return this.HasPower(powerConsumer.powerRelay, power);
        }

        /**
         *
         * Yeterli güç olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasPower(global::PowerRelay powerRelay, float power)
        {
            if (this.IsTechnologyRequiresPower())
            {
                return powerRelay != null && powerRelay.GetPower() >= power;
            }

            return true;
        }

        /**
         *
         * Güç tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConsumePower(global::PowerConsumer powerConsumer, float amount, out float amountConsumed)
        {
            var isConsumed = this.ModifyPower(powerConsumer.powerRelay, -amount, out float modified);
            amountConsumed = -modified;
            return isConsumed;
        }

        /**
         *
         * Güç tüketir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ConsumePower(global::PowerRelay powerRelay, float amount, out float amountConsumed)
        {
            var isConsumed = this.ModifyPower(powerRelay, -amount, out float modified);
            amountConsumed = -modified;
            return isConsumed;
        }

        /**
         *
         * Güç değerlerini düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ModifyPower(global::PowerConsumer powerConsumer, float amount, out float modified)
        {
            return this.ModifyPower(powerConsumer.powerRelay, amount, out modified);
        }

        /**
         *
         * Güç değerlerini düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ModifyPower(global::PowerRelay powerRelay, float amount, out float modified)
        {
            if (this.IsTechnologyRequiresPower())
            {
                modified = 0.0f;

                if (powerRelay.electronicsDisabled)
                {
                    return false;
                }

                if (powerRelay.internalPowerSource)
                {
                    string powerSourceId = this.GetPowerSourceId(powerRelay.internalPowerSource.gameObject);
                    return powerSourceId.IsNotNull() && this.InnerModifyPower(powerSourceId, amount, out modified);
                }
                else
                {
                    var powerRelayId = powerRelay.gameObject.GetIdentityId();
                    return powerRelayId.IsNotNull() && this.ModifyPowerFromInbound(powerRelayId, amount, out modified);
                }
            }

            modified = amount;
            return true;
        }

        /**
         *
         * Güç Relay değerlerini düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ModifyPowerFromInbound(string powerRelayId, float amount, out float modified)
        {
            modified = 0.0f;
            
            var isConsumed = false;

            if (this.PowerRelays.TryGetValue(powerRelayId, out var powerSources))
            {
                foreach (string powerSourceId in powerSources)
                {
                    if (this.PowerRelays.ContainsKey(powerSourceId))
                    {
                        isConsumed = this.ModifyPowerFromInbound(powerSourceId, amount, out float modifiedPower);

                        modified += modifiedPower;
                        amount   -= modifiedPower;

                        if (isConsumed)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        isConsumed = this.InnerModifyPower(powerSourceId, amount, out float modifiedPower);

                        modified += modifiedPower;
                        amount   -= modifiedPower;

                        if (isConsumed)
                        {
                            return true;
                        }
                    }
                }
            }

            return isConsumed;
        }

        /**
         *
         * Güç değerlerini düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool InnerModifyPower(string powerSourceId, float amount, out float modified)
        {
            modified = 0.0f;

            if (!Core.Server.Instance.Storages.Construction.Storage.Constructions.TryGetValue(powerSourceId, out ConstructionItem construction))
            {
                return false;
            }

            var powerSource = Core.Server.Instance.Storages.World.Storage.PowerSources.Where(q => q.ConstructionId == construction.Id).FirstOrDefault();
            if (powerSource == null)
            {
                return false;
            }

            var currentPower = powerSource.Power;
            var isConsumed = amount < 0.0 ? powerSource.Power >= -amount : amount <= powerSource.MaxPower - powerSource.Power;

            powerSource.ModifyPower(amount);

            modified = powerSource.Power - currentPower;
            return isConsumed;
        }

        /**
         *
         * Güç kaynağının id'sini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetPowerSourceId(GameObject powerSource)
        {
            var constructable = powerSource.GetComponent<Constructable>();
            if (constructable)
            {
                return constructable.gameObject.GetIdentityId();
            }

            var bioReactor = powerSource.GetComponent<BaseBioReactor>();
            if (bioReactor)
            {
                return bioReactor.GetModel()?.gameObject.GetIdentityId();
            }

            var nuclearReactor = powerSource.GetComponent<BaseNuclearReactor>();
            if (nuclearReactor)
            {
                return nuclearReactor.GetModel()?.gameObject.GetIdentityId();
            }

            return null;
        }
    }
}

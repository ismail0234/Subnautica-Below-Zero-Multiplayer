namespace Subnautica.Client.Synchronizations.Processors.World
{
    using System.Collections.Generic;

    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.API.Features;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EnergyProductionProcessor : NormalProcessor
    {
        /**
         *
         * Güç kaynaklarını önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<uint, global::PowerSource> PowerSources { get; set; } = new Dictionary<uint, global::PowerSource>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.EnergyTransmissionArgs>();
            if (!World.IsLoaded)
            {
                return false;
            }

            foreach (var construction in packet.PowerSources)
            {
                if (this.PowerSources.TryGetValue(construction.Key, out var powerSource) && powerSource != null)
                {
                    this.AddEnergyToPowerSource(powerSource, construction.Value);
                    continue;
                }

                if (Multiplayer.Constructing.Builder.TryGetBuildingValue(construction.Key, out string uniqueId))
                {
                    var constructable = Network.Identifier.GetComponentByGameObject<Constructable>(uniqueId);
                    if (constructable != null) 
                    {
                        this.PowerSources[construction.Key] = constructable.GetComponent<PowerSource>();
                        this.AddEnergyToPowerSource(this.PowerSources[construction.Key], construction.Value);
                        continue;
                    }

                    var gameObject = Network.Identifier.GetComponentByGameObject<UniqueIdentifier>(uniqueId);
                    if (gameObject.TryGetComponent<BaseBioReactorGeometry>(out var bioReactorGeometry))
                    {
                        var module = bioReactorGeometry.GetModule();
                        if (module != null)
                        {
                            this.PowerSources[construction.Key] = module._powerSource;
                            this.AddEnergyToPowerSource(this.PowerSources[construction.Key], construction.Value);
                        }

                        continue;
                    }

                    if (gameObject.TryGetComponent<BaseNuclearReactorGeometry>(out var nuclearReactorGeometry))
                    {
                        var module = nuclearReactorGeometry.GetModule();
                        if (module != null)
                        {
                            this.PowerSources[construction.Key] = module._powerSource;
                            this.AddEnergyToPowerSource(this.PowerSources[construction.Key], construction.Value);
                        }

                        continue;
                    }
                }
            }
          
            return true;
        }

        /**
         *
         * Güç kaynağındaki enerjiyi değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void AddEnergyToPowerSource(global::PowerSource powerSource, float energy)
        {
            powerSource.power = energy;
        }
    }
}

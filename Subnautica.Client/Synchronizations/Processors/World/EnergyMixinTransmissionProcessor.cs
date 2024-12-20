namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EnergyMixinTransmissionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.EnergyMixinTransmissionArgs>();

            foreach (var item in packet.Items)
            {
                var entity = Network.DynamicEntity.GetEntity(item.ItemId);
                if (entity != null)
                {
                    if (entity.TechType == TechType.Thumper)
                    {
                        var energyMixin = Network.Identifier.GetComponentByGameObject<global::EnergyMixin>(entity.UniqueId);
                        if (energyMixin && energyMixin.battery != null)
                        {
                            energyMixin.battery.charge = item.Charge;
                        }
                    }
                    else if (entity.TechType == TechType.Flare)
                    {
                        var flare = Network.Identifier.GetComponentByGameObject<global::Flare>(entity.UniqueId);
                        if (flare)
                        {
                            flare.energyLeft = item.Charge;

                            if (flare.energyLeft <= 0f)
                            {
                                flare.loopingSound.Stop();

                                Network.DynamicEntity.Remove(entity.UniqueId);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}

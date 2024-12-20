namespace Subnautica.Server.Processors.Building
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BaseHullStrengthProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.BaseHullStrengthTakeDamagingArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
            if (construction == null || construction.BaseId == null)
            {
                return false;
            }

            if (construction.LiveMixin == null)
            {
                construction.LiveMixin = new LiveMixin(packet.MaxHealth, packet.MaxHealth);
            }

            if (construction.LiveMixin.TakeDamage(packet.Damage))
            {
                if (Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent))
                {
                    if (baseComponent.UpdateLeakPoints(construction.UniqueId, construction.LiveMixin.Health, construction.LiveMixin.MaxHealth, packet.LeakPoints))
                    {
                        if (baseComponent.TryGetLeaker(construction.UniqueId, out var leaker))
                        {
                            packet.LeakPoints = leaker.Points;
                        }
                    }
                    else
                    {
                        packet.LeakPoints = null;
                    }

                    packet.CurrentHealth = construction.LiveMixin.Health;

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

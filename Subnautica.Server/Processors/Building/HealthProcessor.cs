namespace Subnautica.Server.Processors.Building
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class HealthProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionHealthArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }          

            var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
            if (construction == null)
            {
                return false;
            }

            if (construction.LiveMixin == null)
            {
                construction.LiveMixin = new LiveMixin(packet.MaxHealth, packet.MaxHealth);
            }

            if (construction.LiveMixin.TakeDamage(packet.Damage))
            {
                if (construction.LiveMixin.IsDead && Server.Instance.Storages.Construction.ConstructionRemove(packet.UniqueId))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

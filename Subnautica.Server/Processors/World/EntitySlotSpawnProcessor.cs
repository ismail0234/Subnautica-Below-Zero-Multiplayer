namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EntitySlotSpawnProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.EntitySlotProcessArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsBreakable)
            {
                if (Server.Instance.Logices.Storage.TryPickupToWorld(packet.WorldPickupItem, out var entity))
                {
                    entity.SetPositionAndRotation(packet.Position, new ZeroQuaternion());
                    entity.SetOwnership(profile.UniqueId);

                    packet.Entity = entity;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

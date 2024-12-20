namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class StaticEntityProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StaticEntityPickedUpArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.UniqueId.IsNull())
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, profile.InventoryItems))
                {
                    packet.Entity = Server.Instance.Storages.World.GetPersistentEntity<StaticEntity>(packet.WorldPickupItem.Item.ItemId);

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Storages.World.AddDisablePersistentEntity(packet.UniqueId))
                {
                    packet.Entity = Server.Instance.Storages.World.GetPersistentEntity<StaticEntity>(packet.UniqueId);

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

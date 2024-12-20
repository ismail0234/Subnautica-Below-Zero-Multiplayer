namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class ExosuitStorageProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ExosuitStorageArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId))
            {
                return false;
            }

            var entity = Server.Instance.Storages.World.GetVehicle(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }

            var vehicle = entity.Component.GetComponent<WorldEntityModel.Exosuit>();
            if (vehicle == null)
            {
                return false;
            }

            vehicle.ResizeStorageContainer();

            if (packet.IsPickup)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, vehicle.StorageContainer))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (packet.IsAdded)
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, vehicle.StorageContainer, profile.InventoryItems))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
                else
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, profile.InventoryItems, vehicle.StorageContainer))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}
namespace Subnautica.Server.Processors.Vehicle
{
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;

    public class SeaTruckAquariumModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckAquariumModuleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId))
            {
                return false;
            }

            var storageContainer = this.GetStorageContainer(packet.UniqueId);
            if (storageContainer == null)
            {
                return false;
            }

            if (packet.IsAdded)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, storageContainer, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.WorldPickupItem, profile.InventoryItems, storageContainer))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Saklama kabını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Metadata.StorageContainer GetStorageContainer(string lockerId)
        {
            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckAquariumModule))
            {
                var component = item.Component.GetComponent<WorldEntityModel.SeaTruckAquariumModule>();
                var locker = component.Lockers.Where(q => q.UniqueId == lockerId).FirstOrDefault();
                if (locker != null)
                {
                    return locker.StorageContainer;
                }
            }

            return null;
        }
    }
}

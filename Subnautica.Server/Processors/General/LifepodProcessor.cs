namespace Subnautica.Server.Processors.General
{
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class LifepodProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.LifepodArgs>();
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
         * Depolamayı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private StorageContainer GetStorageContainer(string uniqueId)
        {
            var lifepod = Server.Instance.Storages.World.Storage.SupplyDrops.Where(q => q.UniqueId == uniqueId).FirstOrDefault();
            if (lifepod == null)
            {
                return null;
            }

            if (lifepod.StorageContainer == null)
            {
                lifepod.StorageContainer = StorageContainer.Create(6, 8);
            }

            return lifepod.StorageContainer;
        }
    }
}

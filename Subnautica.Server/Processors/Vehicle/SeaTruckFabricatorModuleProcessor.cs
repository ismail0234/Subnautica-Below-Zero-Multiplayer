namespace Subnautica.Server.Processors.Vehicle
{
    using System.Linq;

    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckFabricatorModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckFabricatorModuleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var storageContainer = this.GetStorageContainer(packet.UniqueId);
            if (storageContainer == null)
            {
                return false;
            }

            if (packet.IsSignProcess)
            {
                if (packet.IsSignSelect)
                {
                    if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId))
                    {
                        return false;
                    }

                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
                }
                else
                {
                    if (storageContainer.Sign == null)
                    {
                        storageContainer.Sign = new Metadata.Sign();
                    }

                    storageContainer.Sign.Text = packet.SignText;
                    storageContainer.Sign.ColorIndex = packet.SignColorIndex;

                    profile.SendPacketToOtherClients(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId))
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
            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckFabricatorModule).ToList())
            {
                var component = item.Component.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>();
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

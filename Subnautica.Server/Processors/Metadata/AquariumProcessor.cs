namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class AquariumProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
            {
                return false;
            }

            var component = packet.Component.GetComponent<Metadata.Aquarium>();
            if (component == null)
            {
                return false;
            }

            var storageContainer = this.GetStorageContainer(construction);
            if (storageContainer == null)
            {
                return false;
            }

            if (component.IsAdded)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, storageContainer, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, profile.InventoryItems, storageContainer))
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
        private Metadata.StorageContainer GetStorageContainer(ConstructionItem construction)
        {
            var aquarium = construction.EnsureComponent<Metadata.Aquarium>();
            if (aquarium.StorageContainer == null)
            {
                aquarium.StorageContainer = Metadata.StorageContainer.Create(2, 4);
            }

            return aquarium.StorageContainer;
        }
    }
}

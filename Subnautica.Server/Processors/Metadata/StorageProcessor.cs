namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class StorageProcessor : MetadataProcessor
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
            if (packet.SecretTechType == TechType.Sign)
            {
                return this.ProcessSign(profile, packet, construction);
            }

            var locker = packet.Component.GetComponent<Metadata.StorageLocker>();
            if (locker == null)
            {
                return false;
            }

            var storageContainer = this.GetStorageContainer(construction);
            if (storageContainer == null)
            {
                return false;
            }

            if (locker.IsAdded)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(locker.WorldPickupItem, storageContainer, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(locker.WorldPickupItem, profile.InventoryItems, storageContainer))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
 
            return true;
        }

        /**
         *
         * Deponun tabela verisini işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessSign(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            var component = packet.Component.GetComponent<Metadata.Sign>();
            if (component == null)
            {
                return false;
            }

            if (component.IsOpening)
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);
                }
            }
            else if (component.IsSave)
            {
                var storageContainer = this.GetStorageContainer(construction);
                if (storageContainer != null)
                {
                    storageContainer.Sign = component;

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, storageContainer))
                    {
                        packet.TechType = TechType.Sign;

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);
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
            if (construction.Component == null)
            {
                if (construction.TechType == TechType.SmallLocker)
                {
                    construction.Component = Metadata.StorageContainer.Create(5, 6);
                }
                else if (construction.TechType == TechType.Locker)
                {
                    construction.Component = Metadata.StorageContainer.Create(6, 8);
                }
                else
                {
                    return null;
                }
            }

            var component = construction.Component.GetComponent<Metadata.StorageContainer>();
            if (component.Size == 0)
            {
                if (construction.TechType == TechType.SmallLocker)
                {
                    construction.Component = Metadata.StorageContainer.Create(5, 6);
                }
                else if (construction.TechType == TechType.Locker)
                {
                    construction.Component = Metadata.StorageContainer.Create(6, 8);
                }

                return construction.Component.GetComponent<Metadata.StorageContainer>();
            }

            return construction.Component.GetComponent<Metadata.StorageContainer>();
        }
    }
}

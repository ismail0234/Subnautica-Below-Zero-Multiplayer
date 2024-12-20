namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class BioReactorProcessor : MetadataProcessor
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

            var component = packet.Component.GetComponent<Metadata.BioReactor>();
            if (component == null)
            {
                return false;
            }

            var bioReactor = construction.EnsureComponent<Metadata.BioReactor>();
            if (bioReactor == null)
            {
                return false;
            }

            if (component.IsAdded)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, bioReactor.StorageContainer, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

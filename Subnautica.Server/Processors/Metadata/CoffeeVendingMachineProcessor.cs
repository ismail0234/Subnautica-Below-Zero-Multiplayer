namespace Subnautica.Server.Processors.Metadata
{
    using System.Linq;

    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class CoffeeVendingMachineProcessor : MetadataProcessor
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

            var component = packet.Component.GetComponent<Metadata.CoffeeVendingMachine>();
            if (component == null)
            {
                return false;
            }

            var constructionComponent = construction.EnsureComponent<Metadata.CoffeeVendingMachine>();
            if (component.IsAdding)
            {
                if (!profile.IsInventoryItemExists(component.ItemId))
                {
                    return false;
                }

                var thermos = constructionComponent.Thermoses.Where(q => !q.IsActive).FirstOrDefault();
                if (thermos == null)
                {
                    return false;
                }

                profile.RemoveInventoryItem(component.ItemId);

                thermos.Initialize(component.ItemId, Server.Instance.Logices.World.GetServerTime());
            }
            else
            {
                if (profile.IsInventoryItemExists(component.ItemId))
                {
                    return false;
                }

                var thermos = constructionComponent.Thermoses.Where(q => q.IsActive && q.ItemId == component.ItemId).FirstOrDefault();
                if (thermos == null)
                {
                    return false;
                }

                if (component.PickupItem != null)
                {
                    profile.AddInventoryItem(component.PickupItem.GetStorageItem());
                }
                
                component.IsFull = thermos.IsFull;

                thermos.Clear();
            }

            if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
            {
                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

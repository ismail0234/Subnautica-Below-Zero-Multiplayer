namespace Subnautica.Server.Processors.Metadata
{
    using System.Linq;

    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class PlanterProcessor : MetadataProcessor
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

            var component = packet.Component.GetComponent<Metadata.Planter>();
            if (component == null)
            {
                return false;
            }

            if (component.IsOpening)
            {
                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                profile.SendPacket(packet);
            }
            else if (component.CurrentItem != null)
            {
                var constructionComponent = construction.EnsureComponent<Metadata.Planter>();

                if (component.IsAdding)
                {
                    if (constructionComponent.Items.Where(q => q.SlotId == component.CurrentItem.SlotId || q.ItemId == component.CurrentItem.ItemId).Any())
                    {
                        return false;
                    }

                    component.CurrentItem.TimeStartGrowth = Server.Instance.Logices.World.GetServerTime();
                    component.CurrentItem.TimeNextFruit   = component.CurrentItem.TimeStartGrowth + component.CurrentItem.Duration;

                    constructionComponent.Items.Add(component.CurrentItem);

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
                else if (component.IsHarvesting)
                {
                    var item = constructionComponent.Items.Where(q => q.ItemId == component.CurrentItem.ItemId).FirstOrDefault();
                    if (item == null)
                    {
                        return false;
                    }

                    component.CurrentItem.Health = item.Health;

                    if (component.CurrentItem.FruitSpawnInterval == -1)
                    {
                        constructionComponent.Items.Remove(item);

                        if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                    else
                    {
                        item.MaxSpawnableFruit = component.CurrentItem.MaxSpawnableFruit;
                        item.FruitSpawnInterval = component.CurrentItem.FruitSpawnInterval;
                        item.SyncFruits(Server.Instance.Logices.World.GetServerTime());

                        if (item.ActiveFruitCount > 0)
                        {
                            item.ActiveFruitCount--;

                            if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                            {
                                component.CurrentItem = item;

                                profile.SendPacketToAllClient(packet);
                            }
                        }
                    }
                }
                else if (component.CurrentItem.Health != -1f)
                {
                    var item = constructionComponent.Items.Where(q => q.ItemId == component.CurrentItem.ItemId).FirstOrDefault();
                    if (item == null)
                    {
                        return false;
                    }

                    item.Health = component.CurrentItem.Health;

                    component.CurrentItem.SlotId = item.SlotId;

                    if (item.Health <= 0f)
                    {
                        constructionComponent.Items.Remove(item);
                    }

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                    {
                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }

            return true;
        }
    }
}



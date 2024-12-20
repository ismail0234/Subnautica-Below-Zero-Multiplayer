namespace Subnautica.Server.Processors.Metadata
{
    using System.Linq;

    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class NuclearReactorProcessor : MetadataProcessor
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

            var component = packet.Component.GetComponent<Metadata.NuclearReactor>();
            if (component == null)
            {
                return false;
            }

            if (component.Items != null && component.Items.Count > 0)
            {
                var constructionComponent = construction.Component?.GetComponent<Metadata.NuclearReactor>();
                if (constructionComponent == null)
                {
                    constructionComponent = new Metadata.NuclearReactor();
                }

                int totalCount = 4 - constructionComponent.Items.Count;
                if (totalCount > 0)
                {
                    for (int i = 0; i < totalCount; i++)
                    {
                        constructionComponent.Items.Add(TechType.None);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    TechType techType = component.Items.ElementAt(i);

                    if (component.IsRemoving)
                    {
                        if (constructionComponent.Items[i] == TechType.DepletedReactorRod && techType == TechType.DepletedReactorRod)
                        {
                            constructionComponent.Items[i] = TechType.None;
                        }
                    }
                    else
                    {
                        if (constructionComponent.Items[i] == TechType.None && techType == TechType.ReactorRod)
                        {
                            constructionComponent.Items[i] = TechType.ReactorRod;
                        }
                    }
                }

                component.Items = constructionComponent.Items;

                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                {
                    profile.SendPacketToOtherClients(packet);
                }
            }

            return true;
        }
    }
}
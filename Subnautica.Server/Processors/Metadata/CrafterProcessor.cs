namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class CrafterProcessor : MetadataProcessor
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

            var component = packet.Component.GetComponent<Metadata.Crafter>();
            if (component == null)
            {
                return false;
            }

            var constructionComponent = this.GetCrafterComponent(construction);
            if (constructionComponent == null)
            {
                return false;
            }

            component.CrafterClone = constructionComponent;

            if (component.IsOpened)
            {
                if (constructionComponent.Open())
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.IsPickup)
            {
                if (constructionComponent.TryPickup())
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.CraftingTechType != TechType.None)
            {
                if (constructionComponent.Craft(component.CraftingTechType, Server.Instance.Logices.World.GetServerTime(), component.CraftingDuration))
                {
                    Server.Instance.Logices.Crafter.Craft(packet);
                }
            }
            else
            {
                if (constructionComponent.Close())
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Crafter nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Metadata.Crafter GetCrafterComponent(ConstructionItem construction)
        {
            if (construction.TechType == TechType.BaseMapRoom)
            {
                var component = construction.EnsureComponent<Metadata.BaseMapRoom>();
                if (component.Crafter == null)
                {
                    component.Crafter = new Metadata.Crafter();
                }

                return component.Crafter;
            }

            return construction.EnsureComponent<Metadata.Crafter>();
        }
    }
}

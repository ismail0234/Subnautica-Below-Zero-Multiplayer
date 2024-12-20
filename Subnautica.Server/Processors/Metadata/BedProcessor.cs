namespace Subnautica.Server.Processors.Metadata
{
    using System.Linq;

    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using Metadata    = Subnautica.Network.Models.Metadata;

    public class BedProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Bed>();
            if (component == null)
            {
                return false;
            }

            var constructionComponent = construction.EnsureComponent<Metadata.Bed>();
            if (constructionComponent.MaxPlayerCount == 0)
            {
                constructionComponent.MaxPlayerCount = this.GetMaxPlayerCount(construction.TechType);

                for (byte i = 0; i < constructionComponent.MaxPlayerCount; i++)
                {
                    constructionComponent.Sides.Add(new Metadata.BedSideItem());
                }
            }

            if (component.IsSleeping)
            {
                if (Server.Instance.Storages.World.Storage.SkipTimeMode || Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId))
                {
                    return false;
                }

                Server.Instance.Logices.Bed.ClearPlayerBeds(profile.PlayerId);

                var sideIndex = constructionComponent.GetBedEmptySideIndex();
                if (sideIndex == -1)
                {
                    return false;
                }

                var bedSide = constructionComponent.Sides.ElementAt(sideIndex);
                if (bedSide == null)
                {
                    return false;
                }

                bedSide.Sleep(profile.PlayerId, component.CurrentSide.Side, Server.Instance.Logices.World.GetServerTime());

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                {
                    component.Sides = constructionComponent.Sides;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
                {
                    return false;
                }

                var bedSide = constructionComponent.Sides.Where(q => q.PlayerId_v2 == profile.PlayerId).FirstOrDefault();
                if (bedSide == null)
                {
                    return false;
                }

                component.CurrentSide.Side = bedSide.Side;

                bedSide.Standup();

                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.Bed_Standup);

                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                {
                    component.Sides = constructionComponent.Sides;

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Max oyuncu sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetMaxPlayerCount(TechType techType)
        {   
            return 1;
        }
    }
}

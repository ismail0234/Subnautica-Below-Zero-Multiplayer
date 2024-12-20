namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class BenchProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Bench>();
            if (component == null)
            {
                return false;
            }

            if (component.IsSitdown)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId))
                {
                    return false;
                }

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                component.Sitdown(profile.PlayerId);
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
                {
                    return false;
                }

                component.Standup();

                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.Bench_Standup);
            }

            if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, packet.Component))
            {
                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}

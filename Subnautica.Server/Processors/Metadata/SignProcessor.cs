namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class SignProcessor : MetadataProcessor
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
                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, packet.Component))
                {
                    profile.SendPacketToOtherClients(packet);
                }
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);
            }

            return true;
        }
    }
}
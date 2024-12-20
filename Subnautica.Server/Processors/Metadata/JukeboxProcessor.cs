namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    public class JukeboxProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Network.Models.Metadata.JukeboxUsed>();
            if (component != null)
            {
                Server.Instance.Logices.Jukebox.OnDataReceived(profile, packet.UniqueId, component.Data);
            }

            return true;
        }
    }
}

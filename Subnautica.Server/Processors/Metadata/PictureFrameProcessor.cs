namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class PictureFrameProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.PictureFrame>();
            if (component == null)
            {
                return false;
            }

            if (component.IsOpening)
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                    profile.SendPacket(packet);
                }
            }
            else
            {
                if (component.ImageData != null && component.ImageData.Length > 0)
                {
                    component.ImageName = this.GetNewImageName();

                    Server.Instance.Storages.PictureFrame.AddImage(packet.UniqueId, component.ImageName, component.ImageData);

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
        
        /**
         *
         * Resim dosyası adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetNewImageName()
        {
            return string.Format("{0}.jpg", Tools.GetShortUniqueId());
        }
    }
}

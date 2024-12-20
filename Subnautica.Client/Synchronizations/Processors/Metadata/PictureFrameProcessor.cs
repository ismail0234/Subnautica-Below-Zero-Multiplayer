namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class PictureFrameProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.PictureFrame>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::PictureFrame>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            if (component.IsOpening)
            {
                using (EventBlocker.Create(TechType.PictureFrame))
                {
                    gameObject.OnHandClick(null);
                }
            }
            else
            {
                gameObject.MultiplayerSelectImage(component.ImageName, component.ImageData);
            }

            return true;
        }

        /**
         *
         * Resim çerçevesi tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPictureFrameOpening(PictureFrameOpeningEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId))
            {
                SendPacketToServer(ev.UniqueId, null, null, true);
            }
        }

        /**
         *
         * Resim çervesine resim eklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPictureFrameImageSelecting(PictureFrameImageSelectingEventArgs ev)
        {
            ev.IsAllowed = false;

            SendPacketToServer(ev.UniqueId, ev.ImageName, ev.ImageData, false);
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string imageName, byte[] imageData, bool isOpening)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.PictureFrame(imageName, imageData, isOpening),
            };

            NetworkClient.SendPacket(result);
        }
    }
}
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

    public class JukeboxProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Jukebox>();
            if (component == null)
            {
                return false;
            }

            var jukeBox = Network.Identifier.GetComponentByGameObject<global::JukeboxInstance>(uniqueId);
            if (jukeBox == null)
            {
                return false;
            }

            jukeBox.ChangeMusic(component.CurrentPlayingTrack, component.IsPaused, component.RepeatMode, component.IsShuffled, component.Volume, component.Position, component.Length);
            return true;
        }

        /**
         *
         * Şarkı kutusunda veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxUsed(JukeboxUsedEventArgs ev)
        {
            if (!ev.IsSeaTruckModule)
            {
                ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
                {
                    UniqueId  = ev.UniqueId,
                    Component = new Metadata.JukeboxUsed()
                    {
                        Data = ev.Data
                    },
                };

                NetworkClient.SendPacket(result);
            }
        }
    }
}
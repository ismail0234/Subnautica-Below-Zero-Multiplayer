namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class SnowmanProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Snowman>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::Snowman>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            using (EventBlocker.Create(TechType.Snowman))
            {
                gameObject.OnHandClick(null);
            }

            return true;
        }

        /**
         *
         * Kardan adam yok edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSnowmanDestroying(SnowmanDestroyingEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity)
            {
                ev.IsAllowed = false;

                ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
                {
                    UniqueId  = ev.UniqueId,
                    Component = new Metadata.Snowman(),
                };

                NetworkClient.SendPacket(result);
            }
        }
    }
}
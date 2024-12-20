namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class SmallStoveProcessor : MetadataProcessor
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
            if (isSilence)
            {
                return false;
            }

            var component = packet.Component.GetComponent<Metadata.SmallStove>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::ToggleOnClick>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            if (component.IsActive)
            {
                ZeroGame.ToggleClickSwitchOn(gameObject);
            }
            else
            {
                ZeroGame.ToggleClickSwitchOff(gameObject);
            }

            return true;
        }

        /**
         *
         * Ocak nesnesi aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSmallStoveSwitchToggle(SmallStoveSwitchToggleEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = ev.UniqueId,
                Component = new Metadata.SmallStove(ev.SwitchStatus),
            };

            NetworkClient.SendPacket(result);
        }
    }
}
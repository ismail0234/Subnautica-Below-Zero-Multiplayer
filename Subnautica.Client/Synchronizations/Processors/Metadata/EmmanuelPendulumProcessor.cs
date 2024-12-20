namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class EmmanuelPendulumProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.EmmanuelPendulum>();
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
                ZeroGame.ToggleClickSwitchOn(gameObject, isSilence);
            }
            else
            {
                ZeroGame.ToggleClickSwitchOff(gameObject, isSilence);
            }

            return true;
        }

        /**
         *
         * Oyuncak aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEmmanuelPendulumSwitchToggle(EmmanuelPendulumSwitchToggleEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = ev.UniqueId,
                Component = new Metadata.EmmanuelPendulum(ev.SwitchStatus),
            };

            NetworkClient.SendPacket(result);
        }
    }
}
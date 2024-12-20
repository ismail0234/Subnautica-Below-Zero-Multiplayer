namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Collections.Generic;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class SpotLightProcessor : MetadataProcessor
    {
        /**
         *
         * Durumları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, bool> Status = new Dictionary<string, bool>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.SpotLight>();
            if (component == null)
            {
                return false;
            }

            Status[uniqueId] = component.IsPowered;
            SetPowered(uniqueId, component.IsPowered);
            return true;
        }

        /**
         *
         * Spotlight oluştuktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpotLightInitialized(SpotLightInitializedEventArgs ev)
        {
            if (Status.TryGetValue(ev.UniqueId, out var isPowered))
            {
                SetPowered(ev.UniqueId, isPowered);
            }
            else
            {
                if (Multiplayer.Constructing.Builder.GetBuilder(ev.UniqueId) != null)
                {
                    SetPowered(ev.UniqueId, false);
                }
            }
        }

        /**
         *
         * Spotlight oluştuktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetPowered(string uniqueId, bool isActive)
        {
            var gameObject = Network.Identifier.GetComponentByGameObject<global::BaseSpotLight>(uniqueId);
            if (gameObject)
            {
                gameObject.powered = isActive;
            }
        }

        /**
         *
         * Ana menüye dönünce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnDispose()
        {
            Status.Clear();
        }
    }
}
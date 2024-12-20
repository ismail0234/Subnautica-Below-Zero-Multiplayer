namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class DrillableProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkWorldEntityComponent packet, byte requesterId, bool isSpawning)
        {
            var entity = packet.GetComponent<EntityModel.Drillable>();
            if (entity.UniqueId.IsNull())
            {
                return false;
            }

            var drillable = Network.Identifier.GetComponentByGameObject<global::Drillable>(packet.UniqueId, true);
            if (drillable != null)
            {
                drillable.SetHealth(entity.LiveMixin.Health);
            }

            return true;
        }
    }
}
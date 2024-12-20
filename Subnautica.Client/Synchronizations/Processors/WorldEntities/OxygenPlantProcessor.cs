namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using EntityModel = Subnautica.Network.Models.WorldEntity;
    using ServerModel = Subnautica.Network.Models.Server;

    public class OxygenPlantProcessor : WorldEntityProcessor
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
            var entity = packet.GetComponent<EntityModel.OxygenPlant>();
            if (entity.UniqueId.IsNull())
            {
                return false;
            }

            Network.StaticEntity.AddStaticEntity(entity);

            var oxygenPlant = Network.Identifier.GetComponentByGameObject<global::OxygenPlant>(entity.UniqueId, true);
            if (oxygenPlant != null)
            {
                oxygenPlant.start = entity.StartedTime;
                oxygenPlant.UpdateVisuals();
            }

            return true;
        }

        /**
         *
         * Oksijen bitkisine tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOxygenPlantClicking(OxygenPlantClickingEventArgs ev)
        {
            ServerModel.WorldEntityActionArgs result = new ServerModel.WorldEntityActionArgs()
            {
                Entity = new EntityModel.OxygenPlant()
                {
                    UniqueId    = ev.UniqueId,
                    StartedTime = ev.StartedTime,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
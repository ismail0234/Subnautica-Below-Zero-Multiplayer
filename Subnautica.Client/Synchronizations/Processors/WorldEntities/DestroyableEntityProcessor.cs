namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Client.Core;
    using Subnautica.Network.Core.Components;
    using Subnautica.Client.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;
    using EntityModel = Subnautica.Network.Models.WorldEntity;
    using Subnautica.API.Extensions;

    public class DestroyableEntityProcessor : WorldEntityProcessor
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
            var entity = packet.GetComponent<EntityModel.DestroyableEntity>();
            if (entity.UniqueId.IsNull())
            {
                return false;
            }

            Network.StaticEntity.AddStaticEntity(entity);

            var liveMixin = Network.Identifier.GetComponentByGameObject<global::LiveMixin>(entity.UniqueId, true);
            if (liveMixin == null)
            {
                return false;
            }

            if (entity.IsSpawnable)
            {
                liveMixin.health = entity.Health;
            }
            else
            {
                liveMixin.Kill();
            }

            return true;
        }

        /**
         *
         * Bir nesne hasar aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTakeDamaging(TakeDamagingEventArgs ev)
        {
            if (ev.IsStaticWorldEntity && ev.IsDestroyable && ev.Damage > 0f)
            {
                ServerModel.WorldEntityActionArgs request = new ServerModel.WorldEntityActionArgs()
                {
                    Entity = new EntityModel.DestroyableEntity()
                    {
                        UniqueId = ev.UniqueId,
                        Health   = ev.NewHealth,
                    },
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}
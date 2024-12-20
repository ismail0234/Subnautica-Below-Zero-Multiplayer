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

    public class SupplyCrateProcessor : WorldEntityProcessor
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
            var entity = packet.GetComponent<EntityModel.SupplyCrate>();
            if (entity.UniqueId.IsNull())
            {
                return false;
            }

            Network.StaticEntity.AddStaticEntity(entity);

            var supplyCrate = Network.Identifier.GetComponentByGameObject<global::SupplyCrate>(entity.UniqueId, true);
            if (supplyCrate != null && !supplyCrate.open)
            {
                supplyCrate.ToggleOpenState();
            }

            return true;
        }

        /**
         *
         * Tedarik sandığı açıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSupplyCrateOpened(SupplyCrateOpenedEventArgs ev)
        {
            ServerModel.WorldEntityActionArgs result = new ServerModel.WorldEntityActionArgs()
            {
                Entity = new EntityModel.SupplyCrate()
                {
                    UniqueId = ev.UniqueId,
                    IsOpened = true,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
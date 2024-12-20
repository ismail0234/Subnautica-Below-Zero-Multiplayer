namespace Subnautica.Server.Processors.WorldEntities
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class ElevatorProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, WorldEntityActionArgs packet)
        {
            var component = packet.Entity.GetComponent<EntityModel.Elevator>();
            if (component == null)
            {
                return false;
            }

            var entity = Server.Instance.Storages.World.GetPersistentEntity<EntityModel.Elevator>(component.UniqueId);
            if (entity == null)
            {
                entity = new EntityModel.Elevator()
                {
                    UniqueId = component.UniqueId,
                };
            }

            if (Server.Instance.Logices.World.GetServerTime() >= entity.StartTime + Interact.ElevatorCall)
            {
                entity.IsUp      = component.IsUp;
                entity.StartTime = Server.Instance.Logices.World.GetServerTime();
                packet.Entity    = entity;

                if (Server.Instance.Storages.World.SetPersistentEntity(entity))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}

namespace Subnautica.Server.Processors.WorldEntities
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class DestroyableEntityProcessor : WorldEntityProcessor
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
            var component = packet.Entity.GetComponent<EntityModel.DestroyableEntity>();
            if (component == null)
            {
                return false;
            }

            if (component.Health <= 0f)
            {
                component.DisableSpawn();
            }

            if (Server.Instance.Storages.World.SetPersistentEntity(packet.Entity))
            {
                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}

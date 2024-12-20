namespace Subnautica.Client.MonoBehaviours.Entity.Components
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    public class EntityInterpolate
    {
        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (Network.DynamicEntity.ActivatedEntities.Count > 0)
            {
                foreach (var entityId in Network.DynamicEntity.GetActivatedEntityIds())
                {
                    var entity = Network.DynamicEntity.GetEntity(entityId);
                    if (entity == null || entity.IsUsingByPlayer || entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId) || entity.ParentId.IsNotNull())
                    {
                        continue;
                    }

                    entity.Interpolate();
                }
            }
        }
    }
}

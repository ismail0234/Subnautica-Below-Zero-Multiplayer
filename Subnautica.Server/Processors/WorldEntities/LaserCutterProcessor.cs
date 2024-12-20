namespace Subnautica.Server.Processors.WorldEntities
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.WorldEntity;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using UnityEngine;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class LaserCutterProcessor : WorldEntityProcessor
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
            var component = packet.Entity.GetComponent<EntityModel.SealedObject>();
            if (component == null)
            {
                return false;
            }

            if (Server.Instance.Logices.Interact.IsBlocked(component.UniqueId, profile.UniqueId))
            {
                return false;
            }

            if (!Server.Instance.Logices.Interact.IsBlocked(component.UniqueId))
            {
                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, component.UniqueId, true);
            }

            Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.LaserCutterTimeout);

            var entity = Server.Instance.Storages.World.GetPersistentEntity<SealedObject>(component.UniqueId);
            if (entity == null)
            {
                entity = new SealedObject()
                {
                    UniqueId = component.UniqueId,
                };
            }

            if (entity.MaxAmount <= 0)
            {
                entity.MaxAmount = component.MaxAmount;
            }

            entity.Amount = Mathf.Min(entity.Amount + component.Amount, entity.MaxAmount);

            if (Mathf.Approximately(entity.Amount, entity.MaxAmount))
            {
                entity.Amount   = entity.MaxAmount;
                entity.IsSealed = false;
            }
            else
            {
                entity.IsSealed = true;
            }

            if (Server.Instance.Storages.World.SetPersistentEntity(entity))
            {
                packet.Entity = entity;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}
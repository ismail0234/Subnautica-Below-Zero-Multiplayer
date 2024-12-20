namespace Subnautica.Server.Processors.WorldEntities
{
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class BulkheadDoorProcessor : WorldEntityProcessor
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
            if (Server.Instance.Logices.Interact.IsBlocked(packet.Entity.UniqueId))
            {
                return false;
            }
            
            var component = packet.Entity.GetComponent<EntityModel.BulkheadDoor>();
            if (component == null)
            {
                return false;
            }

            if (component.StoryCinematicType != StoryCinematicType.None)
            {
                if (!Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(component.StoryCinematicType.ToString()))
                {
                    if (!Server.Instance.Logices.StoryTrigger.IsCinematicFinished(component.StoryCinematicType.ToString()))
                    {
                        return false;
                    }
                }

                Server.Instance.Logices.StoryTrigger.CompleteTrigger(component.StoryCinematicType.ToString());
            }

            Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.Entity.UniqueId, true);
            Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.BulkheadDoor);

            if (Server.Instance.Storages.World.SetPersistentEntity(packet.Entity))
            {
                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}
